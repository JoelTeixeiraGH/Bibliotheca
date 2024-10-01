import { Button, Dialog, DialogActions, DialogTitle } from '@mui/material';
import { useTranslation } from 'react-i18next';

interface DeleteConfirmationDialogProps {
    isDialogOpen: boolean;
    onDialogClose: () => void;
    onYesButton: () => void;
}

const DeleteConfirmationDialog: React.FC<DeleteConfirmationDialogProps> = ({ isDialogOpen, onDialogClose, onYesButton }) => {
    const { t } = useTranslation();

    const screenName = "components.dialogs.deleteConfirmation.";

    return (
        <>
            {/* Delete Confirmation Dialog */}
            <Dialog
                open={isDialogOpen}
                onClose={onDialogClose}
                maxWidth="xs"
                fullWidth
                PaperProps={{
                    sx: {
                        border: '2px solid red',
                        borderRadius: '10px',
                    },
                }} >
                <DialogTitle style={{ textAlign: 'center', color: 'red', fontSize: 30 }}>
                    {t(screenName + "title")}
                </DialogTitle>
                <DialogActions style={{ justifyContent: 'center' }}>
                    <Button onClick={onDialogClose} variant="contained" style={{ color: 'white', backgroundColor: 'red', border: '2px solid red', fontWeight: 'bold' }}>
                        {t(screenName + "actions.1")}
                    </Button>
                    <Button onClick={onYesButton} variant="contained" style={{ color: 'white', backgroundColor: 'green', border: '2px solid green', fontWeight: 'bold' }}>
                        {t(screenName + "actions.2")}
                    </Button>
                </DialogActions>
            </Dialog>
        </>
    );
}

export default DeleteConfirmationDialog