import GridTable from "@/components/table/GridTable";
import { useAuth } from "@/context/AuthContext";
import { PhysicalBook } from "@/models/PhysicalBook";
import physicalBookService from "@/services/physicalBookService";
import Book from "@mui/icons-material/Book";
import { Box, Button, Divider, Grid, Input, TextField, Typography } from "@mui/material";
import { useEffect, useState } from "react";
import { useTranslation } from "react-i18next";
import { useSearchParams } from "react-router-dom";
import { screenName as topScreen } from "./BooksPage";

const BookCopiesPage = () => {
    const { t } = useTranslation();
    const [queryParameters] = useSearchParams();
    const [isLoading, setIsLoading] = useState(false);
    const [copies, setCopies] = useState<PhysicalBook[]>([]);
    const [newCopyCount, setNewCopyCount] = useState(0);
    const [update, setUpdate] = useState(false);
    const [error, setError] = useState(null);

    const screenName = topScreen + "copiesPage.";

    const { user } = useAuth();

    useEffect(() => {
        setIsLoading(true);

        const isbn: string = parseInt(queryParameters.get("isbn")?.toString() || "0").toString();
        const fetchData = async () => {
            try {
                const response = await physicalBookService.getAllPhysicalBooksByISBNByLibraryId(isbn, Number(user.LibraryId));

                if (response.status === 200) {
                    setCopies(response.data);
                }

            } catch (error: any) {
                setError(error.message);
            }
        };

        fetchData();
        setIsLoading(false);
        setUpdate(false);
    }, [update]);

    const handleAddCopy = async (count: number) => {

        const copiesData = {
            isbn: parseInt(queryParameters.get("isbn")?.toString() || "0").toString(),
            numOfBooks: count,
            libraryId: Number(user.LibraryId),
        }

        try {
            const response = await physicalBookService.createPhysicalBooksByNum(copiesData);

            if (response.status === 200) {
                setUpdate(true);
            }

        } catch (error: any) {
            setError(error.message);
        }

        setUpdate(true);
        setNewCopyCount(0);
    }

    const handleBackClick = () => {
        window.history.back();
    };

    const handleMenuClose = () => { setUpdate(true) };

    return (
        <div>
            <Typography variant="h3" style={{ justifyContent: 'center', marginBottom: '20px' }} >
                <Book sx={{ color: 'black', width: '75px', height: '75px' }} />
                { t(screenName + "title") } {queryParameters.get("isbn")}
            </Typography>
            <Grid container md={12}>
                <Grid item md={10}>
                    <Button variant="contained" color='secondary' onClick={handleBackClick}>
                        { t(screenName + "backButton") }
                    </Button>
                </Grid>
                <Grid item md={2} sx={{ display: 'inline-flex' }}>
                    <TextField
                        type="number"
                        label={ t(screenName + "label") }
                        value={newCopyCount}
                        onChange={(e) => setNewCopyCount(Math.max(1, parseInt(e.target.value, 10) || 1))}
                        sx={{ mr: 1 }} />
                    <Button variant="contained" color="secondary" onClick={() => handleAddCopy(newCopyCount)}>
                        { t(screenName + "addButton") }
                    </Button>
                </Grid>
            </Grid>
            <Divider sx={{ my: 2 }} />
            {
                isLoading ? (
                    <img src="src\assets\loading.svg" alt="Loading" style={{ width: '60px', height: '60px' }} />
                ) : (
                    <>
                        {copies.length != 0 ? (
                            <GridTable rows={copies as []} columnName={'physicalBooks'} onMenuClose={handleMenuClose} />
                        ) : (
                            <>
                                {!error && <h2>No copies found</h2>}
                                {error && <h2>{error}</h2>}
                            </>
                        )}
                    </>
                )
            }
        </div >
    );
}

export default BookCopiesPage;