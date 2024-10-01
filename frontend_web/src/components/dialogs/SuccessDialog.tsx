import {
    Dialog,
    DialogTitle,
    DialogContent,
    Typography,
} from '@mui/material';
import DoneIcon from '@mui/icons-material/Done';
import { useTranslation } from 'react-i18next';

interface SuccessDialogProps {
    isDialogOpen: boolean;
    onDialogClose: () => void;
    msg: string;
}

const SuccessDialog: React.FC<SuccessDialogProps> = ({ isDialogOpen, onDialogClose, msg }) => {
    const { t } = useTranslation();

    const screenName = "components.dialogs.success.";

    return (
        <>
            {/* Success Dialog */}
            <Dialog
                open={isDialogOpen}
                onClose={onDialogClose}
                maxWidth="xs"
                fullWidth
                PaperProps={{
                    sx: {
                        border: '2px solid green',
                        borderRadius: '10px',
                    },
                }} >
                <DialogTitle style={{ textAlign: 'center', color: 'green', fontSize: 70 }}>
                    {t(screenName + "title")}
                </DialogTitle>
                <DialogContent style={{ textAlign: 'center', padding: '20px' }}>
                    <Typography paragraph>
                        {msg}                        
                    </Typography>
                    <DoneIcon style={{ fontSize: 200, color: 'green', borderRadius: '50%', border: '10px solid green', padding: '10px' }} />
                </DialogContent>
            </Dialog>
        </>
    );
}

export default SuccessDialog