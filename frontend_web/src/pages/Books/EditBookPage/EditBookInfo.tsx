import React, { useEffect, useState } from 'react';
import './EditBookPage.css'
import GenericBook from '@/models/GenericBook';
import { FaPlus, FaMinus } from 'react-icons/fa';
import { Button, Container, Row, Col, FormGroup, FormLabel, FormControl, InputGroup, Form } from 'react-bootstrap';
import { languageCodes } from '../../../utils/languageCodes';
import { useSearchParams } from 'react-router-dom';
import genericBookService from '../../../services/genericBookService';
import { screenName as topScreen } from './EditBookPage';
import { useTranslation } from 'react-i18next';

const EditBookInfo: React.FC<{ onInfoSubmit: (info: GenericBook) => void }> = ({ onInfoSubmit }) => {
    const { t } = useTranslation();
    const [queryParameters] = useSearchParams();
    const [isLoading, setIsLoading] = useState(false);
    const [bookInfo, setBookInfo] = useState<GenericBook>({
        isbn: "",
        title: "",
        description: "",
        pageNumber: 0,
        languageAlias: "",
        datePublished: "",
        authorsNames: [],
        categoriesNames: [],
        thumbnail: "",
        smallThumbnail: "",
    });

    const screenName = topScreen + "infoPage.";
    const form = screenName + "form."

    // Função para atualizar o estado ao digitar nos campos
    const handleChange = (e: React.ChangeEvent<HTMLInputElement | HTMLSelectElement>) => {
        const { name, value } = e.target;
        setBookInfo((prevInfo) => ({ ...prevInfo, [name]: value }));
    };

    useEffect(() => {
        if (!isLoading) {
            const isbn: string = queryParameters.get("isbn") || "0";
            const fetchData = async () => {
                try {
                    const book = await genericBookService.getGenericBookByISBN(isbn);
                    setBookInfo(book);
                } catch (error) {
                    console.error(error);
                }
                setIsLoading(true);
            };

            fetchData();
        }
    });

    const [genericBooks, setGenericBooks] = useState<string[]>([""]);

    // Estado local para armazenar as categorias adicionadas dinamicamente
    const [categories, setCategories] = useState<string[]>([""]);

    // Função para adicionar um novo input de autor
    const handleAddGenericBook = () => {
        if (genericBooks.length < 5) {
            setGenericBooks([...genericBooks, ""]);
        }
    };

    const handleRemoveGenericBook = (indexToRemove: number) => {
        setGenericBooks((prevGenericBooks) => {
            // Verificar se há mais de um item antes de remover
            if (prevGenericBooks.length > 1) {
                const updatedGenericBooks = [...prevGenericBooks];
                updatedGenericBooks.splice(indexToRemove, 1);
                return updatedGenericBooks;
            }
            // Se houver apenas um item, não remova
            return prevGenericBooks;
        });
    };

    // Função para atualizar o estado dos autores adicionados dinamicamente
    const handleGenericBookChange = (index: number, value: string) => {
        const updatedGenericBook = [...genericBooks];
        updatedGenericBook[index] = value;
        setGenericBooks(updatedGenericBook);
    };

    // Função para adicionar um novo input de categoria
    const handleAddCategory = () => {
        if (categories.length < 5) {
            setCategories([...categories, ""]);
        }
    };

    // Função para atualizar o estado das categorias adicionadas dinamicamente
    const handleCategoryChange = (index: number, value: string) => {
        const updatedCategories = [...categories];
        updatedCategories[index] = value;
        setCategories(updatedCategories);
    };

    const handleRemoveCategory = (indexToRemove: number) => {
        setCategories((prevCategories) => {
            // Verificar se há mais de um item antes de remover
            if (prevCategories.length > 1) {
                const updatedCategories = [...prevCategories];
                updatedCategories.splice(indexToRemove, 1);
                return updatedCategories;
            }
            // Se houver apenas um item, não remova
            return prevCategories;
        });
    };

    //submeter as informações do livro
    const handleInfoSubmit = () => {
        // Atualize os autores e categorias nas informações do livro
        const updatedInfo: GenericBook = {
            ...bookInfo,
            authorsNames: genericBooks.filter((author) => author.trim() !== ""),
            categoriesNames: categories.filter((category) => category.trim() !== ""),
        };
        //callback com as informações do livro
        onInfoSubmit(updatedInfo);
        try {
            // ... Seu código de chamada à API

            // Limpar os campos após o envio bem-sucedido
            setBookInfo((prevInfo) => ({
                ...prevInfo,
                isbn: '',
                title: '',
                description: '',
                pageNumber: 0,
                languageAlias: '',
                datePublished: '',
                authorsNames: [],
                categoriesNames: [],
                thumbnail: '',
                smallThumbnail: '',
            }));

            // Limpar autores e categorias 
            setGenericBooks([""]);
            setCategories([""]);

        } catch (error) {
            // Lidar com erros, talvez exibindo uma mensagem de erro para o usuário
            console.error("Erro ao adicionar livro:", error);
            alert(t(screenName + "alertErrorMessage"))
        }
    };

    return (
        <>
            <Form>
                <Container>
                    {/* ISBN */}
                    <Row>
                        <Col>
                            <FormGroup>
                                <FormLabel>{ t(form + "isbn") }</FormLabel>
                                <FormControl
                                    type="text"
                                    name="isbn"
                                    value={bookInfo.isbn}
                                    onChange={(e) => handleChange(e as React.ChangeEvent<HTMLInputElement>)}
                                    required
                                    disabled={true}
                                />
                            </FormGroup>
                        </Col>
                        <Col>
                            {/* Publication Date */}
                            <FormGroup>
                                <FormLabel>{ t(form + "pubDate") }</FormLabel>
                                <FormControl
                                    type="date"
                                    name="datePublished"
                                    value={bookInfo.datePublished}
                                    onChange={(e) => handleChange(e as React.ChangeEvent<HTMLInputElement>)}
                                    required
                                />
                            </FormGroup>
                        </Col>
                    </Row>

                    {/* Title */}
                    <FormGroup>
                        <FormLabel>{ t(form + "title") }</FormLabel>
                        <FormControl
                            type="text"
                            name="title"
                            value={bookInfo.title}
                            onChange={(e) => handleChange(e as React.ChangeEvent<HTMLInputElement>)}
                            required
                        />
                    </FormGroup>

                    {/* Language */}
                    <Row>
                        <Col>
                            <FormGroup>
                                <FormLabel>{ t(form + "lan") }</FormLabel>
                                <Form.Select
                                    name="languageAlias"
                                    value={bookInfo.languageAlias}
                                    onChange={(e) => handleChange(e as React.ChangeEvent<HTMLInputElement | HTMLSelectElement>)}
                                >
                                    {languageCodes.map((language) => (
                                        <option key={language.code} value={language.code}>
                                            {language.name}
                                        </option>
                                    ))}
                                </Form.Select>
                            </FormGroup>
                        </Col>
                        <Col>

                            {/* Author */}
                            <FormGroup>
                                <FormLabel>{ t(form + "authors") }</FormLabel>
                                {genericBooks.map((author, index) => (
                                    <InputGroup key={index} className="mb-3">
                                        <FormControl
                                            type="text"
                                            value={author}
                                            onChange={(e) => handleGenericBookChange(index, e.target.value)}
                                        />
                                        <InputGroup.Text>
                                            <Button
                                                variant="outline-secondary"
                                                onClick={() => handleRemoveGenericBook(index)}
                                            >
                                                <FaMinus />
                                            </Button>
                                        </InputGroup.Text>
                                    </InputGroup>
                                ))}
                                <Button variant="outline-secondary" onClick={handleAddGenericBook}>
                                    <FaPlus />
                                    <span>{ t(form + "addAuthors") }</span>
                                </Button>
                            </FormGroup>
                        </Col>
                    </Row>

                    <Row>
                        <Col>
                            {/* Page Number */}
                            <FormGroup>
                                <FormLabel>{ t(form + "numPages") }</FormLabel>
                                <FormControl
                                    type="text"
                                    name="pageNumber"
                                    value={bookInfo.pageNumber}
                                    onChange={(e) => handleChange(e as React.ChangeEvent<HTMLInputElement>)}
                                />
                            </FormGroup>
                        </Col>
                        <Col>
                            {/* Category */}
                            <FormGroup>
                                <FormLabel>{ t(form + "category") }</FormLabel>
                                {categories.map((category, index) => (
                                    <InputGroup key={index} className="mb-3">
                                        <FormControl
                                            type="text"
                                            value={category}
                                            onChange={(e) => handleCategoryChange(index, e.target.value)}
                                        />
                                        <InputGroup.Text>
                                            <Button
                                                variant="outline-secondary"
                                                onClick={() => handleRemoveCategory(index)}
                                            >
                                                <FaMinus />
                                            </Button>
                                        </InputGroup.Text>
                                    </InputGroup>
                                ))}
                                <Button
                                    variant="outline-secondary"
                                    onClick={handleAddCategory}
                                >
                                    <FaPlus />
                                    <span>{ t(form + "addCategories") }</span>
                                </Button>
                            </FormGroup>
                        </Col>
                    </Row>

                    <Row>
                        <FormGroup>
                            <FormLabel>{ t(form + "description") }</FormLabel>
                            <FormControl
                                as="textarea"
                                rows={3}
                                name="description"
                                value={bookInfo.description}
                                onChange={(e) => handleChange(e as React.ChangeEvent<HTMLInputElement>)}
                            />
                        </FormGroup>
                    </Row>
                    <Row>
                        <Col>
                            <FormGroup>
                                <FormLabel>{ t(form + "sThumb") }</FormLabel>
                                <FormControl
                                    type="text"
                                    name="smallThumbnail"
                                    value={bookInfo.smallThumbnail}
                                    onChange={(e) => handleChange(e as React.ChangeEvent<HTMLInputElement>)}
                                />
                            </FormGroup>
                        </Col>
                        <Col>
                            <FormGroup>
                                <FormLabel>{ t(form + "thumb") }</FormLabel>
                                <FormControl
                                    type="text"
                                    name="thumbnail"
                                    value={bookInfo.thumbnail}
                                    onChange={(e) => handleChange(e as React.ChangeEvent<HTMLInputElement>)}
                                />
                            </FormGroup>
                        </Col>
                    </Row>
                    {/* Botão para adicionar o livro e chamar a função de callback */}
                    <Button onClick={handleInfoSubmit}>
                        { t(screenName + "updateButton") }
                    </Button>

                </Container>
            </Form>

        </>
    );
}

export default EditBookInfo;