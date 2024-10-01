import { useAuth } from '@/context/AuthContext';
import { useEffect, useState } from 'react';
import { useTranslation } from 'react-i18next';
import { Container, Typography, Paper, Grid, List, ListItem, ListItemText, Fab } from '@mui/material';
import libraryService from '@/services/libraryService';
import { Library } from '@/models/Library';
import AddIcon from '@mui/icons-material/Add';
import LibrariesPage from '@/pages/Libraries/LibrariesPage';

export const screenName = "pages.LibrarianFrontPage";

function AdminFrontPage() {
    const { t } = useTranslation();
    const [date, setDate] = useState<string>("");
    const { user } = useAuth();
    const [libraries, setLibraries] = useState<Library[]>([]);
    const [isLibrariesPageOpen, setLibrariesPageOpen] = useState(false);

    useEffect(() => {
        const fetchLibraryData = async () => {
            try {
                const response = await libraryService.getAllLibraries();

                if (response.status === 200) {
                    const allLibraries = response.data as Library[];
                    setLibraries(allLibraries);
                }
            } catch (error) {
                console.error(error);
            }
        };

        const today = new Date();
        setDate(today.getDate() + " " + t('months.' + today.getMonth()) + " " + today.getFullYear());

        fetchLibraryData();
    }, []);

    return (
        <>
            <h1>{t(screenName + ".header.greeting")}, Admin:  {user.Name}</h1>
            <p>{date}</p>

            <Container maxWidth="md" style={{ marginTop: '20px' }}>
                <Grid container spacing={3}>
                    <Grid item xs={12}>
                        <Paper elevation={3} style={{ padding: '20px', height: '100%', display: 'flex', flexDirection: 'column', alignItems: 'center', justifyContent: 'center', position: 'relative' }}>
                            <Typography variant="h5" gutterBottom>
                                {t(screenName + ".dashboard.libraries_list")}
                            </Typography>
                            <List>
                                {libraries.map((library) => (
                                    <ListItem key={library.libraryId}>
                                        <ListItemText
                                            primary={library.libraryName}
                                            secondary={`${t(screenName + ".dashboard.alias")}: ${library.libraryAlias}, ${t(screenName + ".dashboard.address")}: ${library.libraryAddress}`}
                                        />
                                    </ListItem>
                                ))}
                            </List>
                            {/* Botão de adição no canto inferior direito dentro do Paper */}
                            <Fab color="secondary" style={{ position: 'absolute', bottom: 16, right: 16 }} onClick={() => setLibrariesPageOpen(true)}>
                                <AddIcon />
                            </Fab>
                            {isLibrariesPageOpen && <LibrariesPage />}
                        </Paper>
                    </Grid>
                </Grid>
            </Container>
        </>
    );
}

export default AdminFrontPage;