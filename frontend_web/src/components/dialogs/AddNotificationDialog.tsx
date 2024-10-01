import { useAuth } from '@/context/AuthContext';
import { NotificationDTO } from '@/models/Notification';
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

interface AddNotificationDialogProps {
    isDialogOpen: boolean;
    onDialogClose: () => void;
    onSubmit: (data: NotificationDTO) => void;
}

const AddNotificationDialog: React.FC<AddNotificationDialogProps> = ({ isDialogOpen, onDialogClose, onSubmit }) => {
    const { t } = useTranslation();
    const { user } = useAuth();

    const [notificationTitle, setNotificationTitle] = useState("");
    const [notificationDescription, setNotificationDescription] = useState("");
    const [notificationDate, setNotificationDate] = useState("");

    const screenName = "components.dialogs.addNotification.";

    const handleDialogClose = () => {
        setNotificationTitle("");
        setNotificationDescription("");
        setNotificationDate('');
    };

    const onSubmitClick = () => {
        const notificationData = {
            notificationTitle: notificationTitle,
            notificationDescription: notificationDescription,
            endDate: new Date(notificationDate),
            libraryId: Number(user.LibraryId)
        };

        onSubmit(notificationData);
        handleDialogClose();
    }

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
                <DialogTitle style={{ textAlign: 'center', fontSize: 30 }}>{t(screenName + "title")}</DialogTitle>
                <DialogContent>
                    <TextField
                        label={t(screenName + "input.title")}
                        value={notificationTitle}
                        onChange={(e) => setNotificationTitle(e.target.value)}
                        fullWidth
                        margin="normal"
                    />
                    <TextField
                        label={t(screenName + "input.description")}
                        value={notificationDescription}
                        onChange={(e) => setNotificationDescription(e.target.value)}
                        fullWidth
                        margin="normal"
                    />
                    <TextField
                        label={t(screenName + "input.date")}
                        value={notificationDate}
                        type="date"
                        onChange={(e) => setNotificationDate(e.target.value)}
                        fullWidth
                        variant="outlined"
                        InputLabelProps={{
                            shrink: true,
                        }}
                        margin="normal"
                    />

                </DialogContent>
                <DialogActions style={{ justifyContent: 'center' }}>
                    <Button onClick={() => { onDialogClose(); }} color="primary" variant="contained">
                        {t(screenName + "actions.1")}
                    </Button>
                    <Button onClick={() => { onSubmitClick(); }} disabled={notificationTitle === '' && notificationDescription === '' && notificationDate === ''} color="secondary" variant="contained">
                        {t(screenName + "actions.2")}
                    </Button>
                </DialogActions>
            </Dialog >

        </>
    );
}

export default AddNotificationDialog