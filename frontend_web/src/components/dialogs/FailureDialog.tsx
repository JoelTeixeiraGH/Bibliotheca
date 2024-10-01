import {
    Dialog,
    DialogTitle,
    DialogContent,
    Typography,
} from '@mui/material';
import CloseIcon from '@mui/icons-material/Close';
import { useTranslation } from 'react-i18next';

interface FailureDialogProps {
    isDialogOpen: boolean;
    onDialogClose: () => void;
    msg: string;
}

const FailureDialog: React.FC<FailureDialogProps> = ({ isDialogOpen, onDialogClose, msg }) => {
    const { t } = useTranslation();

    const screenName = "components.dialogs.failure.";

    return (
        <>
            {/* Failure Dialog */}
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
                <DialogTitle style={{ textAlign: 'center', color: 'red', fontSize: 70 }}>
                    {t(screenName + "title")}
                </DialogTitle>
                <DialogContent style={{ textAlign: 'center', padding: '20px' }}>
                    <Typography paragraph>
                        {msg}
                    </Typography>
                    <CloseIcon style={{ fontSize: 200, color: 'red', borderRadius: '50%', border: '10px solid red', padding: '10px' }} />
                </DialogContent>
            </Dialog>
        </>
    );
}

export default FailureDialog