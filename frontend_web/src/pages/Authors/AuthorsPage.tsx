import { TextField, Typography, Button, Divider } from '@mui/material';
import { Person } from '@mui/icons-material';
import GridTable from '@/components/table/GridTable';
import { useEffect, useState } from 'react';
import { Author } from '@/models/Author';
import authorService from '@/services/authorService';
import InputNameDialog from '@/components/dialogs/InputNameDialog';
import FailureDialog from '@/components/dialogs/FailureDialog';
import SuccessDialog from '@/components/dialogs/SuccessDialog';
import { useTranslation } from 'react-i18next';
import { useAuth } from '@/context/AuthContext';

const AuthorsPage = () => {
    const { t } = useTranslation();
    const [isLoading, setIsLoading] = useState(false);
    const [data, setData] = useState<Author[]>([]);
    const [filteredData, setfilteredData] = useState<Author[]>([]);
    const [searchQuery, setSearchQuery] = useState('');
    const [update, setUpdate] = useState(false);
    const [isSuccessDialogOpen, setSuccessDialogOpen] = useState(false);
    const [isFailureDialogOpen, setFailureDialogOpen] = useState(false);
    const [isInputNameDialogOpen, setInputNameDialogOpen] = useState(false);
    const [authorName, setAuthorName] = useState('');
    const [error, setError] = useState(null);

    const { user } = useAuth();

    const screenName = "pages.AuthorsPage.";
    var msg_failure = "components.dialogs.failure.author";
    var msg_success = "components.dialogs.sucesss.author";

    useEffect(() => {
        setIsLoading(true);

        const fetchData = async () => {
            try {
                const response = await authorService.getAllAuthors();

                if (response.status === 200) {
                    setData(response.data);
                    setfilteredData(response.data);
                }

            } catch (error: any) {
                setError(error.message);
            }
        };

        fetchData();
        setUpdate(false);
        setIsLoading(false);
    }, [update, user]);

    useEffect(() => {
        // Filter data based on the search query
        if (Array.isArray(data)) {
            const filteredAuthors = data.filter(author =>
                author.authorId.toString().includes(searchQuery) ||
                author.authorName.toLowerCase().includes(searchQuery.toLowerCase())
            );
            setfilteredData(filteredAuthors);
        }
    }, [searchQuery, filteredData]);

    const handleAddClick = () => {
        setInputNameDialogOpen(true);
    };

    const handleNameChange = (value: string) => {
        setAuthorName(value);
        return value;
    }

    const handleConfirm = async () => {
        setInputNameDialogOpen(false);

        const authorData = {
            authorId: 0,
            authorName: authorName,
        }

        try {
            const response = await authorService.createAuthor(authorData);

            if (response.status === 201) {
                setSuccessDialogOpen(true);
            } else {
                setFailureDialogOpen(true);
            }

        } catch (error) {
            setFailureDialogOpen(true);
        }
    };

    const handleMenuClose = () => {
        setUpdate(true);
    };

    const handleDialogClose = () => {
        setAuthorName('');
        setInputNameDialogOpen(false);
        setSuccessDialogOpen(false);
        setFailureDialogOpen(false);
        setUpdate(true);
    }

    return (
        <div>
            <Typography variant="h3" style={{ justifyContent: 'center', marginBottom: '20px' }} >
                <Person sx={{ color: 'black', width: '75px', height: '75px' }} />
                {t(screenName + "title")}
            </Typography>
            <Button variant="contained" color='secondary' onClick={handleAddClick}>
                {t(screenName + "addAuthor")}
            </Button>
            <Divider sx={{ my: 2 }} />
            {isLoading ? (
                <img src="src\assets\loading.svg" alt="Loading" style={{ width: '60px', height: '60px' }} />
            ) : (
                <>
                    {data.length != 0 ? (
                        <>
                            <TextField
                                id='searchQuery'
                                name='searchQuery'
                                sx={{ mt: 2, width: '25%' }}
                                label={t(screenName + "query.label")}
                                variant="outlined"
                                value={searchQuery}
                                onChange={(e) => setSearchQuery(e.target.value)} />
                            <GridTable rows={filteredData as []} columnName={'authors'} onMenuClose={handleMenuClose} />
                        </>
                    ) : (
                        <>
                            {!error && <h2>{t(screenName + "query.error")}</h2>}
                            {error && <h2>{error}</h2>}
                        </>
                    )}
                </>
            )}
            <InputNameDialog
                title={t(screenName + "addDialogTitle")}
                isDialogOpen={isInputNameDialogOpen}
                onDialogClose={handleDialogClose}
                onConfirmButton={handleConfirm}
                inputName={handleNameChange}
                initialInputValue={''} />

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
        </div>
    );
}

export default AuthorsPage;