import { Divider, Menu, MenuItem } from '@mui/material';
import { Check, Delete } from '@mui/icons-material';
import React, { useState } from 'react';
import DeleteConfirmationDialog from '@/components/dialogs/DeleteConfirmationDialog';
import SuccessDialog from '@/components/dialogs/SuccessDialog';
import FailureDialog from '@/components/dialogs/FailureDialog';
import { useTranslation } from 'react-i18next';
import { PhysicalBook, PhysicalBookStatus } from '@/models/PhysicalBook';
import physicalBookService from '@/services/physicalBookService';
import { useAuth } from '@/context/AuthContext';

interface PhysicalBooksTableMenuProps {
    selectedRow: PhysicalBook;
    anchorEl: HTMLElement | null;
    onClose: () => void;
}

const PhysicalBooksTableMenu: React.FC<PhysicalBooksTableMenuProps> = ({ selectedRow, anchorEl, onClose }) => {
    const { t } = useTranslation();
    const [isSuccessDialogOpen, setSuccessDialogOpen] = useState(false);
    const [isFailureDialogOpen, setFailureDialogOpen] = useState(false);
    const [isDeleteConfirmationDialogOpen, setDeleteConfirmationDialogOpen] = useState(false);
    const [msg_success, setMsgSucess] = useState("");
    const [msg_failure, setMsgFailure] = useState("");
    const { user } = useAuth();

    const screenName = "components.menus.physicalBooks.";

    const handleDeleteRequest = async () => {
        setDeleteConfirmationDialogOpen(false);

        try {
            const response = await physicalBookService.deletePhysicalBook(selectedRow.physicalBookId);

            if (response.status === 201) {
                setMsgSucess("components.dialogs.success.physicalBooks.delete");
                setSuccessDialogOpen(true);
            } else {
                setMsgFailure("components.dialogs.failure.physicalBooks.delete");
                setFailureDialogOpen(true);
            }

        } catch (error) {
            setMsgFailure("components.dialogs.failure.physicalBooks.delete");
            setFailureDialogOpen(true);
        }

        onClose();
    };

    const handleArrived = async () => {
        try {
            const response = await physicalBookService.updatePhysicalBookStatus(selectedRow.physicalBookId, user.LibraryId);

            if (response.status === 201) {
                setMsgSucess("components.dialogs.success.physicalBooks.arrived");
                setSuccessDialogOpen(true);
            } else {
                setMsgFailure("components.dialogs.failure.physicalBooks.arrived")
                setFailureDialogOpen(true);
            }

        } catch (error) {
            setMsgFailure("components.dialogs.failure.physicalBooks.arrived")
            setFailureDialogOpen(true);
        }

        onClose();
    };

    const handleDeleteClick = () => {
        setDeleteConfirmationDialogOpen(true);
    };

    const handleDialogClose = () => {
        setSuccessDialogOpen(false);
        setFailureDialogOpen(false);
        setDeleteConfirmationDialogOpen(false);
        onClose();
    };

    return (
        <>
            <Menu
                id="rowMenu"
                anchorEl={anchorEl}
                open={Boolean(anchorEl)}
                onClose={onClose} >
                {selectedRow?.physicalBookId && (selectedRow.physicalBookStatus === PhysicalBookStatus.Transfer) && [
                    <MenuItem key={'Arrived'} onClick={handleArrived}>
                        <Check sx={{ marginRight: 1 }} /> {t(screenName + "1")}
                    </MenuItem>,
                    <Divider key={'divider'} />
                ]}
                <MenuItem key={'delete'} onClick={handleDeleteClick}>
                    <Delete sx={{ marginRight: 1 }} /> {t(screenName + "2")}
                </MenuItem>
            </Menu>
            <DeleteConfirmationDialog isDialogOpen={isDeleteConfirmationDialogOpen} onDialogClose={handleDialogClose} onYesButton={handleDeleteRequest} />
            {/* Render SuccessDialog only when isSuccessDialogOpen is true */}
            {isSuccessDialogOpen && (
                <SuccessDialog isDialogOpen={isSuccessDialogOpen} onDialogClose={handleDialogClose} msg={t(msg_success)} />
            )}
            {/* Render FailureDialog only when isFailureDialogOpen is true */}
            {isFailureDialogOpen && (
                <FailureDialog isDialogOpen={isFailureDialogOpen} onDialogClose={handleDialogClose} msg={t(msg_failure)} />
            )}
        </>
    )
}

export default PhysicalBooksTableMenu