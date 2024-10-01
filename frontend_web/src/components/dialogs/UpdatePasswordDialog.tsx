import {
    Dialog,
    DialogTitle,
    DialogContent,
    DialogActions,
    Button,
    TextField,
} from '@mui/material';
import { useState } from 'react';
import { useTranslation } from 'react-i18next';

interface ChangePasswordDialogProps {
    title: string;
    isDialogOpen: boolean;
    onDialogClose: () => void;
    onConfirmButton: () => void;
    currentPasswordChange: (value: string) => void;
    newPasswordChange: (value: string) => void;
    confirmNewPasswordChange: (value: string) => void;
}

const UpdatePasswordDialog: React.FC<ChangePasswordDialogProps> = ({
    isDialogOpen,
    onDialogClose,
    onConfirmButton,
    currentPasswordChange: currentPasswordChange,
    newPasswordChange: newPasswordChange,
    confirmNewPasswordChange: confirmNewPasswordChange
}) => {
    const { t } = useTranslation();
    const [currentPassword, setCurrentPassword] = useState('');
    const [newPassword, setNewPassword] = useState("");
    const [confirmNewPassword, setConfirmNewPassword] = useState("");

    const screenName = "components.dialogs.updatePassword.";

    const onCurrentPasswordChange = (event: React.ChangeEvent<HTMLInputElement>) => {
        const newValue = event.target.value;
        setCurrentPassword(newValue);
        currentPasswordChange(newValue);
    };

    const onNewPasswordChange = (event: React.ChangeEvent<HTMLInputElement>) => {
        const newValue = event.target.value;
        setNewPassword(newValue);
        newPasswordChange(newValue);
    };

    const onConfirmNewPasswordChange = (event: React.ChangeEvent<HTMLInputElement>) => {
        const newValue = event.target.value;
        setConfirmNewPassword(newValue);
        confirmNewPasswordChange(newValue);
    };

    const resetParams = () => {
        setCurrentPassword("");
        setNewPassword("");
        setConfirmNewPassword("");
    };

    return (
        <>
            {/* Input Name Dialog */}
            <Dialog
                open={isDialogOpen}
                onClose={onDialogClose}
                maxWidth="xs"
                fullWidth
                PaperProps={{
                    sx: {
                        borderRadius: '10px',
                    },
                }} >
                <DialogTitle style={{ textAlign: 'center', fontSize: 30 }}>{screenName + "title"}</DialogTitle>
                <DialogContent>
                    <TextField
                        fullWidth
                        id="current"
                        label={t(screenName + "currentPassword")}
                        type="password"
                        value={currentPassword}
                        onChange={onCurrentPasswordChange}
                        sx={{ mt: 2 }} />
                </DialogContent>
                <DialogContent>
                    <TextField
                        fullWidth
                        id="new"
                        label={t(screenName + "newPassword")}
                        type="password"
                        value={newPassword}
                        onChange={onNewPasswordChange}
                        sx={{ mt: 2 }} />
                </DialogContent>
                <DialogContent>
                    <TextField
                        fullWidth
                        id="confirmNew"
                        label={t(screenName + "confirmNewPassword")}
                        type="password"
                        value={confirmNewPassword}
                        onChange={onConfirmNewPasswordChange}
                        sx={{ mt: 2 }} />
                </DialogContent>
                <DialogActions style={{ justifyContent: 'center' }}>
                    <Button onClick={() => { onDialogClose(); resetParams(); }} color="primary" variant="contained">
                        {t(screenName + "actions.1")}
                    </Button>
                    <Button onClick={() => { onConfirmButton(); resetParams(); }}
                        disabled={newPassword === ''} color="secondary" variant="contained">
                        {t(screenName + "actions.2")}
                    </Button>
                </DialogActions>
            </Dialog>
        </>
    );
}

export default UpdatePasswordDialog;