import React, { useState } from 'react';
import './AddBookPage.css'
import { GenericBook } from '@/models/GenericBook';
import { FaPlus, FaMinus, FaBook } from 'react-icons/fa';
import { languageCodes } from '../../../utils/languageCodes';
import { screenName as topScreen } from './AddBookPage';
import { useTranslation } from 'react-i18next';
import FailureDialog from '@/components/dialogs/FailureDialog';
import SuccessDialog from '@/components/dialogs/SuccessDialog';
import Container from '@mui/material/Container';
import Grid from '@mui/material/Grid';
import Typography from '@mui/material/Typography';
import TextField from '@mui/material/TextField';
import Select from '@mui/material/Select';
import MenuItem from '@mui/material/MenuItem';
import InputLabel from '@mui/material/InputLabel';
import InputAdornment from '@mui/material/InputAdornment';
import IconButton from '@mui/material/IconButton';
import Button from '@mui/material/Button';
import Box from '@mui/material/Box';

const AddBookInfo: React.FC<{ onInfoSubmit: (info: GenericBook) => void }> = ({ onInfoSubmit }) => {
    const { t } = useTranslation();
    const [isSuccessDialogOpen, setSuccessDialogOpen] = useState(false);
    const [isFailureDialogOpen, setFailureDialogOpen] = useState(false);

    const [msg_success, setMsgSucess] = useState("components.dialogs.success.book.create");
    const [msg_failure, setMsgFailure] = useState("components.dialogs.failure.book.create");

    const screenName = topScreen + "infoPage.";
    const form = screenName + "form."


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

    // Função para atualizar o estado ao digitar nos campos
    const handleChange = (e: React.ChangeEvent<HTMLInputElement | HTMLSelectElement>) => {
        const { name, value } = e.target;
        setBookInfo((prevInfo) => ({ ...prevInfo, [name]: value }));
        //console.log(e.target)
    };


    const [authors, setAuthors] = useState<string[]>([""]);

    // Estado local para armazenar as categorias adicionadas dinamicamente
    const [categories, setCategories] = useState<string[]>([""]);

    // Função para adicionar um novo input de autor
    const handleAddAuthor = () => {
        if (authors.length < 5) {
            setAuthors([...authors, ""]);
        }
    };

    const handleRemoveAuthor = (indexToRemove: number) => {
        setAuthors((prevAuthors) => {
            // Verificar se há mais de um item antes de remover
            if (prevAuthors.length > 1) {
                const updatedAuthors = [...prevAuthors];
                updatedAuthors.splice(indexToRemove, 1);
                return updatedAuthors;
            }
            // Se houver apenas um item, não remova
            return prevAuthors;
        });
    };

    // Função para atualizar o estado dos autores adicionados dinamicamente
    const handleAuthorChange = (index: number, value: string) => {
        const updatedAuthors = [...authors];
        updatedAuthors[index] = value;
        setAuthors(updatedAuthors);
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
            authorsNames: authors.filter((author) => author.trim() !== ""),
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
            setAuthors([""]);
            setCategories([""]);

            setSuccessDialogOpen(true);

        } catch (error) {
            // Lidar com erros, talvez exibindo uma mensagem de erro para o usuário
            setFailureDialogOpen(true);
            alert(t(screenName + "alertErrorMessage"))
        }
    };

    const handleDialogClose = () => {
        setAuthors([""]);
        setCategories([""]);
        setSuccessDialogOpen(false);
        setFailureDialogOpen(false);
    }

    return (
        <>
            <Container>
                <Box mt={4}>
                    {/* ISBN, Publication Date, and Title */}
                    <Grid container spacing={2}>
                        <Grid item xs={2}>
                            <TextField
                                label="ISBN"
                                type="text"
                                name="isbn"
                                value={bookInfo.isbn}
                                onChange={(e) => handleChange(e as React.ChangeEvent<HTMLInputElement>)}
                                required
                                fullWidth
                            />
                        </Grid>
                        <Grid item xs={6}>
                            <TextField
                                label="Title"
                                type="text"
                                name="title"
                                value={bookInfo.title}
                                onChange={(e) => handleChange(e as React.ChangeEvent<HTMLInputElement>)}
                                fullWidth
                                required
                                inputProps={{ style: { textTransform: 'capitalize' } }} // Ajusta o estilo para dar ênfase
                            />
                        </Grid>
                        <Grid item xs={4}>
                            <TextField
                                label="Publication Date"
                                type="date"
                                name="datePublished"
                                value={bookInfo.datePublished}
                                onChange={(e) => handleChange(e as React.ChangeEvent<HTMLInputElement>)}
                                required
                                fullWidth
                                InputLabelProps={{ shrink: true }}
                            />
                        </Grid>
                    </Grid>

                    {/* Language, Author, and Page Number */}
                    <Grid container spacing={2} mt={2}>
                        <Grid item xs={4}>
                            <Select
                                label={t(form + "lan")}
                                name="languageAlias"
                                value={bookInfo.languageAlias}
                                onChange={(e) => handleChange(e as React.ChangeEvent<HTMLInputElement | HTMLSelectElement>)}
                                fullWidth
                            >
                                {languageCodes.map((language) => (
                                    <MenuItem key={language.code} value={language.code}>
                                        {language.name}
                                    </MenuItem>
                                ))}
                            </Select>
                        </Grid>


                        <Grid item xs={4}>
                            {authors.map((author, index) => (
                                <Grid container spacing={1} key={index} alignItems="center">
                                    <Grid item xs={10} mb={1}>
                                        <TextField
                                            label={t(form + "authors")}
                                            type="text"
                                            value={author}
                                            onChange={(e) => handleAuthorChange(index, e.target.value)}
                                            fullWidth
                                        />
                                    </Grid>
                                    <Grid item xs={2}>
                                        <IconButton variant="outlined" onClick={() => handleRemoveAuthor(index)}>
                                            <FaMinus fontSize="small" />
                                        </IconButton>
                                    </Grid>
                                </Grid>
                            ))}
                            <Button variant="outlined" onClick={handleAddAuthor} mt={1}>
                                <FaPlus />
                                <span>{t(form + "addAuthors")}</span>
                            </Button>
                        </Grid>
                        <Grid item xs={4}>
                            <TextField
                                label={t(form + "numPages")}
                                type="text"
                                name="pageNumber"
                                value={bookInfo.pageNumber}
                                onChange={(e) => handleChange(e as React.ChangeEvent<HTMLInputElement>)}
                                fullWidth

                            />
                        </Grid>
                    </Grid>

                    {/* Category, Description, and Thumbnails */}
                    <Grid container spacing={2} mt={2}>
                        <Grid item xs={4}>
                            {categories.map((category, index) => (
                                <Grid container spacing={1} key={index} alignItems="center">
                                    <Grid item xs={10} mb={1}>
                                        <TextField
                                            label={t(form + "category")}
                                            type="text"
                                            value={category}
                                            onChange={(e) => handleCategoryChange(index, e.target.value)}
                                            fullWidth
                                        />
                                    </Grid>
                                    <Grid item xs={2}>
                                        <IconButton variant="outlined" onClick={() => handleRemoveCategory(index)}>
                                            <FaMinus fontSize="small" />
                                        </IconButton>
                                    </Grid>
                                </Grid>
                            ))}
                            <Button variant="outlined" onClick={handleAddCategory} mt={1}>
                                <FaPlus />
                                <span>{t(form + "addCategories")}</span>
                            </Button>
                        </Grid>
                        <Grid item xs={4}>
                            <TextField
                                label={t(form + "description")}
                                multiline
                                rows={3}
                                name="description"
                                value={bookInfo.description}
                                onChange={(e) => handleChange(e as React.ChangeEvent<HTMLInputElement>)}
                                fullWidth
                            />
                        </Grid>
                        <Grid item xs={4}>
                            {/* Thumbnails */}
                            <Grid container spacing={1}>
                                <Grid item xs={12}>
                                    <TextField
                                        label={t(form + "sThumb")}
                                        type="text"
                                        name="smallThumbnail"
                                        value={bookInfo.smallThumbnail}
                                        onChange={(e) => handleChange(e as React.ChangeEvent<HTMLInputElement>)}
                                        fullWidth
                                    />
                                </Grid>
                                <Grid item xs={12}>
                                    <TextField
                                        label={t(form + "thumb")}
                                        type="text"
                                        name="thumbnail"
                                        value={bookInfo.thumbnail}
                                        onChange={(e) => handleChange(e as React.ChangeEvent<HTMLInputElement>)}
                                        fullWidth
                                    />
                                </Grid>
                            </Grid>
                        </Grid>
                    </Grid>

                    {/* Submit Button */}
                    <Button variant="contained" color="secondary" onClick={handleInfoSubmit} mt={2}>
                        {t(screenName + "addButton")}
                    </Button>
                </Box>
            </Container>




            {/* Render SuccessDialog only when isSuccessDialogOpen is true */}
            {isSuccessDialogOpen && (
                <SuccessDialog
                    isDialogOpen={isSuccessDialogOpen}
                    onDialogClose={handleDialogClose}
                    msg={t(msg_success)}
                />
            )}

            {/* Render FailureDialog only when isFailureDialogOpen is true */}
            {isFailureDialogOpen && (
                <FailureDialog
                    isDialogOpen={isFailureDialogOpen}
                    onDialogClose={handleDialogClose}
                    msg={t(msg_failure)}
                />
            )}

        </>
    );
}

export default AddBookInfo;