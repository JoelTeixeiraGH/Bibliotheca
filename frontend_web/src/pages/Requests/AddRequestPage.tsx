import { useEffect, useState } from 'react';
import {
    Avatar,
    Typography,
    Checkbox,
    FormControlLabel,
    Box,
    Card,
    CardHeader,
    CardContent,
    Grid,
    Divider,
} from '@mui/material';
import userService from '@/services/userService';
import { User } from '@/models/User';
import { GenericBook } from '@/models/GenericBook';
import { PhysicalBook } from '@/models/PhysicalBook';
import requestService from '@/services/requestService';
import { useNavigate } from 'react-router-dom';
import SuccessDialog from '@/components/dialogs/SuccessDialog';
import FailureDialog from '@/components/dialogs/FailureDialog';
import Step1Form from '@/components/forms/Step1Form';
import Step2Form from '@/components/forms/Step2Form';
import Step3Form from '@/components/forms/Step3Form';
import Step4Form from '@/components/forms/Step4Form';
import { useAuth } from '@/context/AuthContext';
import Assignment from '@mui/icons-material/Assignment';
import { useTranslation } from 'react-i18next';
import { screenName as topScreen } from './RequestsPage';

const AddRequestPage = () => {
    const { t } = useTranslation();
    const [activeStep, setActiveStep] = useState(1);
    const [users, setUsers] = useState<User[]>([]);
    const [isUsersLoading, setIsUsersLoading] = useState(false);
    const [selectedUser, setSelectedUser] = useState<User | null>(null);
    const [selectedGenericBook, setSelectedGenericBook] = useState<GenericBook | null>(null);
    const [isPhysicalBooksEmpty, setIsPhysicalBooksEmpty] = useState(false);
    const [selectedPhysicalBook, setSelectedPhysicalBook] = useState<PhysicalBook | null>(null);
    const [isWaiting, setIsWaiting] = useState(false);
    const [endDate, setEndDate] = useState<string>('');
    const [error, setError] = useState(false);

    const [isSuccessDialogOpen, setSuccessDialogOpen] = useState(false);
    const [isFailureDialogOpen, setFailureDialogOpen] = useState(false);

    const [msg_success, setMsgSucess] = useState("");
    const [msg_failure, setMsgFailure] = useState("");

    const navigate = useNavigate();

    const { user } = useAuth();

    const screenName = topScreen + "addPage."

    useEffect(() => {
        setIsUsersLoading(true);

        const fetchUsers = async () => {
            try {
                const response = await userService.getAllUsersByLibraryId(user.LibraryId);

                if (response.status === 200) {
                    setUsers(response.data);
                }

            } catch (error: any) {
                setError(error.message);
            }
        };

        setIsUsersLoading(false);
        fetchUsers();
    }, [])

    useEffect(() => {
        if (selectedUser) {
            setSelectedPhysicalBook(null);
            setIsWaiting(false);
            setEndDate('');
            setActiveStep(2);
        }
    }, [selectedUser]);

    useEffect(() => {

        if (selectedGenericBook) {
            setActiveStep(2);
            setSelectedPhysicalBook(null);
            setIsWaiting(false);
            setEndDate('');
        }

    }, [selectedGenericBook]);

    useEffect(() => {

        if (selectedPhysicalBook) {
            setActiveStep(3);
        }

    }, [selectedPhysicalBook]);

    const handleWaitingToggle = () => {
        setIsWaiting((prevValue) => !prevValue);
        if (isWaiting) {
            setActiveStep(2)
        } else {
            setActiveStep(3)
        }
    };

    const handleSubmit = async () => {

        const requestData = {
            endDate: endDate,
            userId: selectedUser?.userId,
            physicalBookId: selectedPhysicalBook?.physicalBookId,
            isbn: selectedGenericBook?.isbn,
            libraryId: user.LibraryId
        }

        // Create request based on the isWaiting value
        if (isWaiting) {
            // Remove properties when isWaiting is true
            const { endDate, physicalBookId, ...requestDataForWaitingList } = requestData;

            try {
                // Create Request with Status Waiting
                var response = await requestService.createRequestWithStatusWaiting(requestDataForWaitingList);

                if (response.status === 201) {
                    setMsgSucess("components.dialogs.success.requests.create")
                    setSuccessDialogOpen(true);
                } else if (response.status === 500) {
                    setMsgFailure("components.dialogs.failure.error.500")
                    setFailureDialogOpen(true);
                } else {
                    setMsgFailure("components.dialogs.failure.requests.create")
                    setFailureDialogOpen(true);
                }

            } catch (error) {
                setFailureDialogOpen(true);
            }
        } else {
            try {
                // Create Request with Status Requested
                var response = await requestService.createRequestWithStatusRequested(requestData);

                if (response.status === 201) {
                    setMsgSucess("components.dialogs.success.requests.create")
                    setSuccessDialogOpen(true);
                } else if (response.status === 500) {
                    setMsgFailure("components.dialogs.failure.error.500")
                    setFailureDialogOpen(true);
                } else {
                    setMsgFailure("components.dialogs.failure.requests.create")
                    setFailureDialogOpen(true);
                }

            } catch (error) {
                setFailureDialogOpen(true);
            }
        }
    };

    const isSubmitable = () => {
        if (isWaiting) {
            return true;
        }
        if (!isWaiting && endDate != '') {
            return true;
        }
        return false;
    }

    const handleDialogClose = () => {
        setSuccessDialogOpen(false);
        setFailureDialogOpen(false);
        setSelectedGenericBook(null);
        setSelectedPhysicalBook(null);
        setIsWaiting(false);
        setEndDate('');
        setActiveStep(1);
    };

    const handleBackButtonClick = () => {
        navigate('/requests');
    };

    const handleIsPhysicalBooksEmpty = (value: boolean) => {
        setIsPhysicalBooksEmpty(value);
        return value;
    };

    return (
        <Box sx={{ maxWidth: '100%' }}>
            <Typography variant="h3" style={{ justifyContent: 'center', marginBottom: '20px' }} >
                <Assignment sx={{ color: 'black', width: '75px', height: '75px' }} />
                {t(screenName + "title")}
            </Typography>
            <Divider sx={{ my: 2 }} />
            <Grid container spacing={2}>
                <Grid item xs={4}>
                    {activeStep >= 1 && (
                        <Card sx={{ marginTop: 2, padding: 2, border: '1px solid black', height: '291px' }}>
                            <CardHeader
                                avatar={<Avatar sx={{ backgroundColor: '#F89300', color: 'black', fontWeight: 'bold' }} > 1 </Avatar>}
                                titleTypographyProps={{ variant: 'h6', fontWeight: 'bold' }}
                                title={t(screenName + "step1.title")} />
                            <Divider sx={{ mb: 2 }} />
                            <CardContent>
                                {isUsersLoading ? (
                                    <img src="src\assets\loading.svg" alt="Loading" style={{ width: '60px', height: '60px' }} />
                                ) : (
                                    <>
                                        {users.length != 0 ? (
                                            <Step1Form<User>
                                                options={users}
                                                getOptionLabel={(user: User) => `${user.userId} | ${user.userEmail}`}
                                                onChange={(_e, selectedValue) => setSelectedUser(selectedValue)}
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
                                    step1Choice={selectedUser}
                                    libraryId={user.LibraryId}
                                    onGenericBookChange={(value) => setSelectedGenericBook(value)}
                                    isPhysicalBooksEmpty={handleIsPhysicalBooksEmpty}
                                    onPhysicalBookChange={(value) => setSelectedPhysicalBook(value)} />
                                {!selectedPhysicalBook && isPhysicalBooksEmpty && (
                                    <FormControlLabel
                                        control={<Checkbox checked={isWaiting} onChange={handleWaitingToggle} />}
                                        label={t(screenName + "step2.error")}
                                        sx={{ mt: 2 }} />
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
                                title={t(screenName + "step3.title")} />
                            <Divider sx={{ mb: 2 }} />
                            <CardContent>
                                <Step3Form showEndDate={!isWaiting} onEndDate={setEndDate} />
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
        </Box>
    );
}

export default AddRequestPage;