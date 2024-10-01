import FailureDialog from "@/components/dialogs/FailureDialog";
import SuccessDialog from "@/components/dialogs/SuccessDialog";
import UpdatePasswordDialog from "@/components/dialogs/UpdatePasswordDialog";
import { useAuth } from "@/context/AuthContext";
import { UpdatePasswordDTO } from "@/models/User";
import libraryService from "@/services/libraryService";
import userService from "@/services/userService";
import { Button, Divider, Grid, Typography } from "@mui/material";
import axios from "axios";
import { useEffect, useState } from "react";
import { useTranslation } from "react-i18next";
import { useSearchParams } from "react-router-dom";

const ProfilePage = () => {
    const { t } = useTranslation();
    const [queryParameters] = useSearchParams();
    const [isLoading, setIsLoading] = useState(false);
    const [error, setError] = useState(null);
    const [isPasswordDialogOpen, setPasswordDialogOpen] = useState(false);
    const [currentPassword, setCurrentPassword] = useState("");
    const [newPassword, setNewPassword] = useState("");
    const [confirmNewPassword, setConfirmNewPassword] = useState("");
    const [isSuccessDialogOpen, setSuccessDialogOpen] = useState(false);
    const [isFailureDialogOpen, setFailureDialogOpen] = useState(false);
    const [update, setUpdate] = useState(false);
    const [libraryAlias, setLibraryAlias] = useState("");
    const [msg_failure, setFailureMessage] = useState("message");

    const { user } = useAuth();

    const screenName = "pages.ProfilePage.";
    const msg_failure_base = "components.dialogs.failure.password.";
    const msg_success = "components.dialogs.success.password";

    useEffect(() => {
        setIsLoading(true);

        const fetchData = async () => {
            try {
                const response = await libraryService.getLibraryById(user.LibraryId as number);
                if (response.status === 200) {
                    setLibraryAlias(response.data.libraryAlias);
                }

            } catch (error: any) {
                setError(error.message);
            }
        };

        setIsLoading(false);
        fetchData();
    }, []);

    const handleBackClick = () => {
        window.history.back();
    };

    const handleDialogClose = () => {
        setCurrentPassword("");
        setNewPassword("");
        setPasswordDialogOpen(false);
        setSuccessDialogOpen(false);
        setFailureDialogOpen(false);
        setUpdate(true);
    }

    const handleCurrentPasswordChange = (value: string) => {
        setCurrentPassword(value);
        return value;
    }

    const handleNewPasswordChange = (value: string) => {
        setNewPassword(value);
        return value;
    }

    const handleConfirmNewPasswordChange = (value: string) => {
        setConfirmNewPassword(value);
        return value;
    }

    const handleConfirm = async () => {
        setPasswordDialogOpen(false);

        if (newPassword !== confirmNewPassword) {
            setFailureMessage(msg_failure_base + "1");
            setFailureDialogOpen(true);
            return;
        }

        const updateData: UpdatePasswordDTO = {
            oldPassword: currentPassword,
            newPassword: newPassword
        }

        try {
            const response = await userService.updateUserPassword(user.Id, updateData);

            if (response.status === 201) {
                setSuccessDialogOpen(true);
            } else {
                
                setFailureDialogOpen(true);
            }

        } catch (error) {
            const status = error.message.slice(-3);
            console.log("Status: " + status)
            switch (status) {
                case "400":
                    setFailureMessage(msg_failure_base + "2");
                    break;
                case "401":
                    setFailureMessage(msg_failure_base + "3");
                    break;
                default:
                    setFailureMessage(msg_failure_base + "3");
                    break;
            }
            setFailureDialogOpen(true);
        }
    }

    const informationArray = [
        { name: t(screenName + "infoArray.name"), value: user?.Name },
        { name: t(screenName + "infoArray.email"), value: user?.Email },
        { name: t(screenName + "infoArray.libraryAlias"), value: libraryAlias }
    ];

    return (
        <div>
            <Typography variant="h3" style={{ justifyContent: 'center', marginBottom: '20px' }} >
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
                    {user != undefined ? (
                        <div>
                            <Grid container spacing={2}>
                                { /* Talvez adicionar aqui imagem da escola ou do utilizador
                                <Grid item xs={12} md={3}>
                                    <img src={genericBook?.thumbnail} alt={genericBook?.title} style={{ width: '100%', height: 'auto' }} />
                                </Grid> */}
                                <Grid item xs={12} md={9}>
                                    <Grid container spacing={2}>
                                        {informationArray.map((info, index) => (
                                            <Grid key={index} item xs={12} sx={{ display: 'flex' }}>
                                                <Typography variant="h5" sx={{ mr: 1, color: 'black', fontWeight: 'bold' }}>{info.name}:</Typography>
                                                <Typography variant="h6">{info.value}</Typography>
                                            </Grid>
                                        ))}
                                    </Grid>
                                    <Divider sx={{ my: 2 }} />
                                    <Button onClick={() => setPasswordDialogOpen(true)}>{t(screenName + "updatePasswordButton")}</Button>
                                </Grid>
                            </Grid>
                        </div>
                    ) : (
                        <>
                            {!error && <h2>{t(screenName + "error")}</h2>}
                            {error && <h2>{error}</h2>}
                        </>
                    )}
                </>
            )}
            <UpdatePasswordDialog
                title={t(screenName + "dialogTitle")}
                isDialogOpen={isPasswordDialogOpen}
                onDialogClose={handleDialogClose}
                onConfirmButton={handleConfirm}
                currentPasswordChange={handleCurrentPasswordChange}
                newPasswordChange={handleNewPasswordChange}
                confirmNewPasswordChange={handleConfirmNewPasswordChange}
            />

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
    )
}

export default ProfilePage;