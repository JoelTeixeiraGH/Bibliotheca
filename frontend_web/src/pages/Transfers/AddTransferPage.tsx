import { useEffect, useState } from "react";
import {
    Avatar,
    Box,
    Card,
    CardContent,
    CardHeader,
    Typography,
    Grid,
    Divider
} from "@mui/material";
import libraryService from "@/services/libraryService";
import { Library } from "@/models/Library";
import { GenericBook } from '@/models/GenericBook';
import { PhysicalBook } from "@/models/PhysicalBook";
import transferService from "@/services/transferService";
import Step1Form from "@/components/forms/Step1Form";
import Step2Form from "@/components/forms/Step2Form";
import Step3Form from "@/components/forms/Step3Form";
import Step4Form from "@/components/forms/Step4Form";
import SuccessDialog from "@/components/dialogs/SuccessDialog";
import FailureDialog from "@/components/dialogs/FailureDialog";
import { useNavigate } from "react-router-dom";
import { useAuth } from "@/context/AuthContext";
import SwapHoriz from "@mui/icons-material/SwapHoriz";
import { useTranslation } from "react-i18next";
import { screenName as topScreen } from "./TransfersPage";

const AddTransferPage = () => {
    const { t } = useTranslation();
    const [activeStep, setActiveStep] = useState(1);
    const [libraries, setLibraries] = useState<Library[]>([]);
    const [isLibrariesLoading, setIsLibrariesLoading] = useState(false);
    const [selectedLibrary, setSelectedLibrary] = useState<Library | null>(null);
    const [selectedGenericBook, setSelectedGenericBook] = useState<GenericBook | null>(null);
    const [isPhysicalBooksEmpty, setIsPhysicalBooksEmpty] = useState(false);
    const [selectedPhysicalBook, setSelectedPhysicalBook] = useState<PhysicalBook | null>(null);
    const [endDate, setEndDate] = useState<string>('');
    const [error, setError] = useState(false);

    const [isSuccessDialogOpen, setSuccessDialogOpen] = useState(false);
    const [isFailureDialogOpen, setFailureDialogOpen] = useState(false);
    const navigate = useNavigate();

    const { user } = useAuth();

    const [msg_success, setMsgSucess] = useState("");
    const [msg_failure, setMsgFailure] = useState("");

    const screenName = topScreen + "addPage."

    useEffect(() => {
        setIsLibrariesLoading(true);

        const fetchLibraries = async () => {
            try {
                const response = await libraryService.getAllLibraries();

                if (response.status === 200) {
                    setLibraries(response.data.filter((library: Library) => library.libraryId != user.LibraryId));
                }

            } catch (error: any) {
                setError(error.message);
            }
        };

        setIsLibrariesLoading(false);
        fetchLibraries();
    }, []);

    useEffect(() => {
        if (selectedLibrary) {
            setSelectedPhysicalBook(null);
            setEndDate('');
            setActiveStep(2);
        }
    }, [selectedLibrary]);

    useEffect(() => {

        if (selectedGenericBook) {
            setActiveStep(2);
            setSelectedPhysicalBook(null);
            setEndDate('');
        }

    }, [selectedGenericBook]);

    useEffect(() => {

        if (selectedPhysicalBook) {
            setActiveStep(3);
        }

    }, [selectedPhysicalBook]);

    const handleSubmit = async () => {

        const transferData = {
            endDate: endDate,
            sourceLibraryId: Number(selectedLibrary?.libraryId),
            destinationLibraryId: user.LibraryId,
            physicalBookId: Number(selectedPhysicalBook?.physicalBookId),
        }

        try {
            var response = await transferService.createTransfer(transferData);

            if (response.status === 201) {
                setMsgSucess("components.dialogs.success.transfer.create");
                setSuccessDialogOpen(true);
            } else if (response.status === 500) {
                setMsgFailure("components.dialogs.failure.error_500");
                setFailureDialogOpen(true);
            } else {
                setMsgSucess("components.dialogs.failure.transfer.create");
                setFailureDialogOpen(true);
            }

        } catch (error) {
            setFailureDialogOpen(true);
        }
    }

    const isSubmitable = () => {
        if (endDate != '') {
            return true;
        }
        return false;
    }

    const handleDialogClose = () => {
        setSuccessDialogOpen(false);
        setFailureDialogOpen(false);
        setSelectedGenericBook(null);
        setSelectedPhysicalBook(null);
        setEndDate('');
        setActiveStep(1);
    };

    const handleBackButtonClick = () => {
        navigate('/transfers');
    };

    const handleIsPhysicalBooksEmpty = (value: boolean) => {
        setIsPhysicalBooksEmpty(value);
        return value;
    };

    return (
        <Box sx={{ maxWidth: '100%' }}>
            <Typography variant="h3" style={{ justifyContent: 'center', marginBottom: '20px' }} >
                <SwapHoriz sx={{ color: 'black', width: '75px', height: '75px' }} />
                {t(screenName + "title")}
            </Typography>
            <Divider sx={{ my: 2 }} />
            <Grid container spacing={2}>
                <Grid item xs={4} >
                    {activeStep >= 1 && (
                        <Card sx={{ marginTop: 2, padding: 2, border: '1px solid black', height: '291px' }}>
                            <CardHeader
                                avatar={<Avatar sx={{ backgroundColor: '#F89300', color: 'black', fontWeight: 'bold' }} > 1 </Avatar>}
                                titleTypographyProps={{ variant: 'h6', fontWeight: 'bold' }}
                                title="Select source library" />
                            <Divider sx={{ mb: 2 }} />
                            <CardContent>
                                {isLibrariesLoading ? (
                                    <img src="src\assets\loading.svg" alt="Loading" style={{ width: '60px', height: '60px' }} />
                                ) : (
                                    <>
                                        {libraries.length != 0 ? (
                                            <Step1Form<Library>
                                                options={libraries}
                                                getOptionLabel={(library: Library) => `${library.libraryId} | ${library.libraryAlias}`}
                                                onChange={(_e, selectedValue) => setSelectedLibrary(selectedValue)}
                                                label={t(screenName + "step1.title")} />
                                        ) : (
                                            <>
                                                {!error && <h2>{t(screenName + "step1.error")}</h2>}
                                                {error && <h2>{error}</h2>}
                                            </>
                                        )}
                                    </>
                                )}
                            </CardContent>
                        </Card>
                    )}
                </Grid>
                <Grid item xs={4}>
                    {activeStep >= 2 && (
                        <Card sx={{ marginTop: 2, padding: 2, border: '1px solid black', height: '291px' }}>
                            <CardHeader
                                avatar={<Avatar sx={{ backgroundColor: '#F89300', color: 'black', fontWeight: 'bold' }} > 2 </Avatar>}
                                titleTypographyProps={{ variant: 'h6', fontWeight: 'bold' }}
                                title={t(screenName + "step2.title")} />
                            <Divider sx={{ mb: 2 }} />
                            <CardContent>
                                <Step2Form
                                    step1Choice={selectedLibrary}
                                    libraryId={selectedLibrary?.libraryId ?? 0}
                                    onGenericBookChange={(value) => setSelectedGenericBook(value)}
                                    isPhysicalBooksEmpty={handleIsPhysicalBooksEmpty}
                                    onPhysicalBookChange={(value) => setSelectedPhysicalBook(value)} />
                                {!selectedPhysicalBook && isPhysicalBooksEmpty && (
                                    <Typography variant="h6" style={{ justifyContent: 'center', marginTop: 8 }}>
                                        {t(screenName + "step2.error")}
                                    </Typography>
                                )}
                            </CardContent>
                        </Card>
                    )}
                </Grid>
                <Grid item xs={4}>
                    {activeStep === 3 && (
                        <Card sx={{ marginTop: 2, padding: 2, border: '1px solid black', height: '291px' }}>
                            <CardHeader
                                avatar={<Avatar sx={{ backgroundColor: '#F89300', color: 'black', fontWeight: 'bold' }}> 3 </Avatar>}
                                titleTypographyProps={{ variant: 'h6', fontWeight: 'bold' }}
                                title="Select transfer deadline" />
                            <Divider sx={{ mb: 2 }} />
                            <CardContent>
                                <Step3Form showEndDate={true} onEndDate={setEndDate} />
                            </CardContent>
                        </Card>
                    )}
                </Grid>
                <Grid item xs={4}></Grid>
                <Grid item xs={4} style={{ display: 'flex', justifyContent: 'center' }}>
                    <Step4Form onBackButton={handleBackButtonClick} onSubmitButton={handleSubmit} isSubmitable={isSubmitable()} />
                </Grid>
                <Grid item xs={4}></Grid>
            </Grid>
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
        </Box >
    );
}

export default AddTransferPage;