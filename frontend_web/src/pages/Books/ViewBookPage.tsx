import { useAuth } from "@/context/AuthContext";
import { GenericBook } from "@/models/GenericBook";
import genericBookService from "@/services/genericBookService";
import Book from "@mui/icons-material/Book";
import { Button, Card, CardActions, CardContent, Divider, Grid, IconButton, Rating, Typography } from "@mui/material";
import DeleteIcon from "@mui/icons-material/Delete";
import { useEffect, useState } from "react";
import { useTranslation } from "react-i18next";
import { useSearchParams } from "react-router-dom";
import { screenName as topScreen } from "./BooksPage";
import evaluationService from "@/services/evaluationService";
import DeleteConfirmationDialog from "@/components/dialogs/DeleteConfirmationDialog";
import SuccessDialog from "@/components/dialogs/SuccessDialog";
import FailureDialog from "@/components/dialogs/FailureDialog";

const ViewBookPage = () => {
    const { t } = useTranslation();
    const [queryParameters] = useSearchParams();
    const [isLoading, setIsLoading] = useState(false);
    const [genericBook, setGenericBook] = useState<GenericBook | undefined>(undefined);
    const [error, setError] = useState(null);
    const [update, setUpdate] = useState(false);
    const [isDeleteConfirmationDialogOpen, setDeleteConfirmationDialogOpen] = useState(false);
    const [isSuccessDialogOpen, setSuccessDialogOpen] = useState(false);
    const [isFailureDialogOpen, setFailureDialogOpen] = useState(false);
    const [selectedId, setSelectedId] = useState(0);
    const [msg_success, setMsgSucess] = useState("");
    const [msg_failure, setMsgFailure] = useState("");

    const screenName = topScreen + "viewPage.";

    const { user } = useAuth();

    useEffect(() => {
        setIsLoading(true);

        const isbn: string = parseInt(queryParameters.get("isbn")?.toString() || "0").toString();
        const fetchData = async () => {
            try {
                const response = await genericBookService.getGenericBookByISBN(isbn);

                if (response.status === 200) {
                    setGenericBook(response.data);
                }

            } catch (error: any) {
                setError(error.message);
            }
        };

        setIsLoading(false);
        setUpdate(false);
        fetchData();
    }, [update]);

    const handleDelete = async () => {
        try {
            const response = await evaluationService.deleteEvaluation(selectedId);

            if (response.status === 204) {
                setSuccessDialogOpen(true);
            } else {
                setFailureDialogOpen(true);
                setError(response.data);
            }

        } catch (error: any) {
            setFailureDialogOpen(true);
            setError(error.message);
        }
    };

    const handleDeleteClick = (id: number) => {
        setSelectedId(id);
        setDeleteConfirmationDialogOpen(true);
    };

    const availableCopies = genericBook?.physicalBooks?.find(library => library.libraryId === Number(user.LibraryId))?.count || 0;

    const informationArray = [
        { name: t(screenName + "infoArray.isbn"), value: genericBook?.isbn },
        { name: t(screenName + "infoArray.pageNumber"), value: genericBook?.pageNumber },
        { name: t(screenName + "infoArray.pubDate"), value: genericBook?.datePublished },
        { name: t(screenName + "infoArray.lan"), value: genericBook?.language.languageAlias },
        { name: t(screenName + "infoArray.authors"), value: genericBook?.authors.map((author: any) => author.authorName).join(', ') },
        { name: t(screenName + "infoArray.categories"), value: genericBook?.categories.map((category: any) => category.categoryName).join(', ') },
        { name: t(screenName + "infoArray.availability"), value: availableCopies },
        { name: t(screenName + "infoArray.numberOfEvaluations"), value: genericBook?.numberOfEvaluations }
    ];

    const handleBackClick = () => {
        window.history.back();
    };

    const handleDialogClose = () => {
        setSuccessDialogOpen(false);
        setFailureDialogOpen(false);
        setDeleteConfirmationDialogOpen(false);
        setUpdate(true);
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
            {isLoading ? (
                <img src="src\assets\loading.svg" alt="Loading" style={{ width: '60px', height: '60px' }} />
            ) : (
                <>
                    {genericBook != undefined ? (
                        <>
                            <Grid container spacing={2}>
                                <Grid item xs={12} md={3}>
                                    <img src={genericBook?.thumbnail} alt={genericBook?.title} style={{ width: '100%', height: 'auto' }} />
                                </Grid>
                                <Grid item xs={12} md={9}>
                                    <Typography gutterBottom variant="h4" sx={{ color: 'black', fontWeight: 'bold' }}>{genericBook?.title}</Typography>
                                    <Divider sx={{ my: 2 }} />
                                    <Typography variant="h5" sx={{ mr: 1, color: 'black', fontWeight: 'bold' }}>{t(screenName + "description")}</Typography>
                                    <Typography gutterBottom variant="h6">{genericBook?.description}</Typography>
                                    <Grid container spacing={2}>
                                        {informationArray.map((info, index) => (
                                            <Grid key={index} item xs={12} sx={{ display: 'flex' }}>
                                                <Typography variant="h5" sx={{ mr: 1, color: 'black', fontWeight: 'bold' }}>{info.name}:</Typography>
                                                <Typography variant="h6">{info.value}</Typography>
                                            </Grid>
                                        ))}
                                        <Grid item xs={12} sx={{ display: 'flex' }}>
                                            <Typography variant="h5" sx={{ color: 'black', fontWeight: 'bold' }}>
                                                {t(screenName + "infoArray.averageEvaluationScore")}:
                                                <Rating
                                                    name={`score-1`}
                                                    value={genericBook?.averageEvaluationScore}
                                                    readOnly
                                                />
                                            </Typography>
                                        </Grid>
                                    </Grid>
                                </Grid>
                            </Grid>
                            <Divider sx={{ my: 2 }} />
                            <Typography variant="h4" sx={{ mr: 1, color: 'black', fontWeight: 'bold' }}>{t(screenName + "review.title")}</Typography>
                            <Divider sx={{ my: 2 }} />
                            {genericBook?.evaluations?.map((evaluation, index) => (
                                <div key={index}>
                                    <Card sx={{ width: "100%" }}>
                                        <CardContent>
                                            <Grid item xs={12} sx={{ display: 'flex' }}>
                                                <Typography variant="h5" sx={{ mr: 1, color: 'black', fontWeight: 'bold' }}>{t(screenName + "review.name")}: </Typography>
                                                <Typography variant="h6">{evaluation.userName}</Typography>
                                                <div style={{ flex: 1 }} />
                                                <Typography variant="h5" sx={{ color: 'black', fontWeight: 'bold' }}>
                                                    {t(screenName + "review.score")}:
                                                    <Rating
                                                        name={`score-${index}`}
                                                        value={evaluation.evaluationScore}
                                                        precision={1} // You can adjust precision as needed
                                                        readOnly
                                                    />
                                                </Typography>
                                            </Grid>
                                            <Divider sx={{ mb: 2 }} />
                                            <Typography variant="h5" sx={{ mr: 1, color: 'black', fontWeight: 'bold' }}>{t(screenName + "review.description")}: </Typography>
                                            <Typography variant="h6">{evaluation.evaluationDescription}</Typography>
                                            <Grid item xs={12} sx={{ display: 'flex' }}>
                                                <Typography variant="h5" sx={{ mr: 1, color: 'black', fontWeight: 'bold' }}>{t(screenName + "review.date")}: </Typography>
                                                <Typography variant="h6">{evaluation.emittedDate}</Typography>
                                            </Grid>
                                        </CardContent>
                                        <CardActions>
                                            <IconButton size="small" sx={{ color: "red", fontWeight: "bold" }} onClick={() => handleDeleteClick(evaluation.evaluationId)}>
                                                <DeleteIcon />{t(screenName + "review.delete")}
                                            </IconButton>
                                        </CardActions>
                                    </Card>
                                    <Divider sx={{ my: 1 }} />
                                </div>
                            ))}

                        </>
                    ) : (
                        <>
                            {!error && <h2>No Book found</h2>}
                            {error && <h2>{error}</h2>}
                        </>
                    )}
                </>
            )}
            <DeleteConfirmationDialog isDialogOpen={isDeleteConfirmationDialogOpen} onDialogClose={handleDialogClose} onYesButton={handleDelete} />
            {/* Render SuccessDialog only when isSuccessDialogOpen is true */}
            {isSuccessDialogOpen && (
                <SuccessDialog isDialogOpen={isSuccessDialogOpen} onDialogClose={handleDialogClose} msg={t(msg_success)} />
            )}
            {/* Render FailureDialog only when isFailureDialogOpen is true */}
            {isFailureDialogOpen && (
                <FailureDialog isDialogOpen={isFailureDialogOpen} onDialogClose={handleDialogClose} msg={t(msg_failure)} />
            )}
        </div >
    );
}

export default ViewBookPage;