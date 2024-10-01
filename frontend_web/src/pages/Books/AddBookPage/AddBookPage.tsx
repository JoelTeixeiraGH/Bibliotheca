import React, { useState } from 'react';
import AddBookInfo from './AddBookInfo';
import './AddBookPage.css';
import { FaBook } from 'react-icons/fa';
import genericBookService from '../../../services/genericBookService';
import { GenericBook } from '@/models/GenericBook';
import { Button, Divider, Grid, Typography } from "@mui/material";
import Book from "@mui/icons-material/Book";
import { useTranslation } from 'react-i18next';
import { screenName as topScreen } from '../BooksPage';

export const screenName = topScreen + "addPage.";

function AddBookPage() {
    const { t } = useTranslation();

    const handleInfoSubmit = async (info: GenericBook) => {
        try {
            // Chame o serviço/API para enviar as informações do livro
            await genericBookService.createGenericBook(info);

        } catch (error: any) {
            console.error(error.message);

        }
    };

    const handleBackClick = () => {
        window.history.back();
    };

    return (
        <div>
            <Typography variant="h3" style={{ justifyContent: 'center', marginBottom: '20px' }} >
                <Book sx={{ color: 'black', width: '75px', height: '75px' }} />
                {t(screenName + "title")}
            </Typography>
            <Button variant="contained" color='secondary' onClick={handleBackClick}>
                {t(screenName + "backButton")}
            </Button>
            <Divider sx={{ my: 2 }} />
            <Grid container justifyContent="center" mt={4}>
                <Grid item>
                    {/* Passa a função de callback para lidar com as informações do livro */}
                    <AddBookInfo onInfoSubmit={handleInfoSubmit} />
                </Grid>
            </Grid>
        </div>
    );
}

export default AddBookPage;