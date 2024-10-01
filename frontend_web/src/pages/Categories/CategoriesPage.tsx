import { Button, Divider, TextField, Typography } from '@mui/material';
import CategoryIcon from '@mui/icons-material/Category';
import GridTable from '@/components/table/GridTable';
import { useEffect, useState } from 'react';
import { Category } from '@/models/Category';
import categoryService from '@/services/categoryService';
import InputNameDialog from '@/components/dialogs/InputNameDialog';
import FailureDialog from '@/components/dialogs/FailureDialog';
import SuccessDialog from '@/components/dialogs/SuccessDialog';
import { useTranslation } from 'react-i18next';

const CategoriesPage = () => {
    const { t } = useTranslation();
    const [isLoading, setIsLoading] = useState(false);
    const [data, setData] = useState<Category[]>([]);
    const [filteredData, setfilteredData] = useState<Category[]>([]);
    const [searchQuery, setSearchQuery] = useState('');
    const [update, setUpdate] = useState(false);
    const [isSuccessDialogOpen, setSuccessDialogOpen] = useState(false);
    const [isFailureDialogOpen, setFailureDialogOpen] = useState(false);
    const [isInputNameDialogOpen, setInputNameDialogOpen] = useState(false);
    const [categoryName, setCategoryName] = useState('');
    const [error, setError] = useState(null);
    const msg_success = "components.dialogs.success.category.create";
    const [msg_failure, setMsgFailure] = useState("components.dialogs.failure.category.create");

    const screenName = "pages.CategoriesPage.";


    useEffect(() => {
        setIsLoading(true);

        const fetchData = async () => {
            try {
                const response = await categoryService.getAllCategories();

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
    }, [update]);

    useEffect(() => {
        // Filter data based on the search query
        if (Array.isArray(data)) {
            const filteredCategories = data.filter(category =>
                category.categoryId.toString().includes(searchQuery) ||
                category.categoryName.toLowerCase().includes(searchQuery.toLowerCase())
            );
            setfilteredData(filteredCategories);
        }
    }, [searchQuery, filteredData, data]);

    const handleAddClick = () => {
        setInputNameDialogOpen(true);
    };

    const handleNameChange = (value: string) => {
        setCategoryName(value);
        return value;
    }

    const handleConfirm = async () => {
        setInputNameDialogOpen(false);

        setMsgFailure("components.dialogs.failure.category.create");

        const categoryData = {
            categoryId: 0,
            categoryName: categoryName,
        }

        try {
            const response = await categoryService.createCategory(categoryData);

            if (response.status === 201) {
                setSuccessDialogOpen(true);
            } else if (response.status === (400 | 500)) {
                setMsgFailure("components.dialogs.failure.error." + response.status);
                setFailureDialogOpen(true);
            } else {
                setMsgFailure("An unknown error has occurred")
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
        setCategoryName('');
        setInputNameDialogOpen(false);
        setSuccessDialogOpen(false);
        setFailureDialogOpen(false);
        setUpdate(true);
    }

    return (
        <div>
            <Typography variant="h3" style={{ justifyContent: 'center', marginBottom: '20px' }} >
                <CategoryIcon sx={{ color: 'black', width: '75px', height: '75px' }} />
                {t(screenName + "title")}
            </Typography>
            <Button variant="contained" color='secondary' onClick={handleAddClick}>
                {t(screenName + "addButton")}
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
                            <GridTable rows={filteredData as []} columnName={'categories'} onMenuClose={handleMenuClose} />
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

export default CategoriesPage;