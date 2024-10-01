import React, { useState } from 'react';
import EditBookInfo from "./EditBookInfo";
import './EditBookPage.css';
import { FaBook } from 'react-icons/fa';
import bookService from '../../../services/genericBookService';
import { GenericBook } from '@/models/GenericBook';
import { Alert, Container, Row, Col } from 'react-bootstrap';
import { useTranslation } from 'react-i18next';
import { screenName as topScreen } from '../BooksPage';

export const screenName = topScreen + "editPage."

function EditBookPage() {
    const { t } = useTranslation();
    const [successMessage, setSuccessMessage] = useState('');
    const [errorMessage, setErrorMessage] = useState('');

    const handleInfoSubmit = async (info: GenericBook) => {
        try {
            // Chame o serviço/API para enviar as informações do livro
            await bookService.updateGenericBook(info);

            // Lógica adicional após a criação bem-sucedida
            setSuccessMessage(t(screenName + "successMessage"));
            //console.log(info)
            setErrorMessage(''); // Limpa a mensagem de erro
        } catch (error: any) {
            console.error(error.message);
            // Lógica adicional para lidar com erros
            setSuccessMessage('');
            setErrorMessage(`${error.message}`);
        }
    };

    const handleAlertClose = () => {
        setSuccessMessage('');
        setErrorMessage('');
    };

    return (
        <Container>
            <Row className="justify-content-center mt-4">
                <Col xs="auto">
                    <FaBook size={30} />
                </Col>
            </Row>
            <Row className="justify-content-center mt-2">
                <Col xs="auto">
                    <span className="description">{ t(screenName + "title") }</span>
                </Col>
            </Row>
            <Row className="justify-content-center mt-4">
                <Col>
                    {/* Passa a função de callback para lidar com as informações do livro */}
                    <EditBookInfo onInfoSubmit={handleInfoSubmit} />
                    {successMessage && (
                        <Alert variant="success" onClick={handleAlertClose}>
                            {successMessage}
                        </Alert>
                    )}
                    {errorMessage && (
                        <Alert variant="danger" onClick={handleAlertClose}>
                            {errorMessage}
                        </Alert>
                    )}
                </Col>
            </Row>
        </Container>
    );
}

export default EditBookPage;