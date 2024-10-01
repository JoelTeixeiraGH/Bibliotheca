import { Divider, Menu, MenuItem } from '@mui/material';
import { Cancel, ThumbUp, ThumbDown, Delete } from '@mui/icons-material';
import React, { useEffect, useState } from 'react';
import DeleteConfirmationDialog from '@/components/dialogs/DeleteConfirmationDialog';
import SuccessDialog from '@/components/dialogs/SuccessDialog';
import FailureDialog from '@/components/dialogs/FailureDialog';
import transferService from '@/services/transferService';
import { Transfer, TransferStatus } from '@/models/Transfer';
import { useAuth } from '@/context/AuthContext';
import { useTranslation } from 'react-i18next';

interface TransfersTableMenuProps {
    selectedRow: Transfer;
    anchorEl: HTMLElement | null;
    onClose: () => void;
}

const TransfersTableMenu: React.FC<TransfersTableMenuProps> = ({ selectedRow, anchorEl, onClose }) => {
    const { t } = useTranslation();
    const [isSuccessDialogOpen, setSuccessDialogOpen] = useState(false);
    const [isFailureDialogOpen, setFailureDialogOpen] = useState(false);
    const [isDeleteConfirmationDialogOpen, setDeleteConfirmationDialogOpen] = useState(false);
    const [msg_success, setMsgSucess] = useState("");
    const [msg_failure, setMsgFailure] = useState("");


    const screenName = "components.menus.transfers.";

    const { user } = useAuth();

    const handleDeleteTransfer = async () => {
        setDeleteConfirmationDialogOpen(false);

        try {
            const response = await transferService.deleteTransfer(selectedRow.transferId);

            if (response.status === 201) {
                setMsgSucess("components.dialogs.success.transfer.delete")
                setSuccessDialogOpen(true);
            } else if (response.status === 500) {
                setMsgFailure("components.dialogs.failure.error_500")
                setFailureDialogOpen(true);
            } else {
                setMsgFailure("components.dialogs.failure.transfer.delete")
                setFailureDialogOpen(true);
            }
        } catch (error) {
            setFailureDialogOpen(true);
        }

        onClose();
    };

    const handleDeleteClick = () => {
        setDeleteConfirmationDialogOpen(true);
    };

    const handleChangeToCancelTransfer = async () => {
        try {
            const response = await transferService.changeTransferStatusToCanceled(selectedRow.transferId);

            if (response.status === 201) {
                setMsgSucess("components.dialogs.success.transfer.cancel")
                setSuccessDialogOpen(true);
            } else {
                setMsgFailure("components.dialogs.failure.transfer.cancel")
                setFailureDialogOpen(true);
            }

        } catch (error: any) {
            if (error.response.status === 500) {
                setMsgFailure("components.dialogs.failure.error_500")
                setFailureDialogOpen(true);
            }
        }

        onClose();
    };

    const handleChangeToAcceptedTransfer = async () => {
        try {
            const response = await transferService.changeTransferStatusToAccepted(selectedRow.transferId);

            if (response.status === 201) {
                setMsgSucess("components.dialogs.success.transfer.accepted")
                setSuccessDialogOpen(true);
            } else {
                setMsgFailure("components.dialogs.failure.transfer.accepted")
                setFailureDialogOpen(true);
            }

        } catch (error: any) {
            if (error.response.status === 500) {
                setMsgFailure("components.dialogs.failure.error_500")
                setFailureDialogOpen(true);
            }
        }

        onClose();
    };

    const handleChangeToRejectedTransfer = async () => {
        try {
            const response = await transferService.changeTransferStatusToRejected(selectedRow.transferId);

            if (response.status === 201) {
                setMsgSucess("components.dialogs.success.transfer.reject")
                setSuccessDialogOpen(true);
            } else {
                setMsgFailure("components.dialogs.failure.transfer.reject")
                setFailureDialogOpen(true);
            }

        } catch (error: any) {
            if (error.response.status === 500) {
                setMsgFailure("components.dialogs.failure.error_500")
                setFailureDialogOpen(true);
            }
        }

        onClose();
    };

    const handleDialogClose = () => {
        setSuccessDialogOpen(false);
        setFailureDialogOpen(false);
        setDeleteConfirmationDialogOpen(false);
    };


    return (
        <>
            <Menu
                id="rowMenu"
                anchorEl={anchorEl}
                open={Boolean(anchorEl)}
                onClose={onClose} >
                {selectedRow?.transferId && Number(user.LibraryId) === selectedRow?.destinationLibrary.destinationLibraryId && selectedRow?.transferStatus === TransferStatus.Pending && (
                    <MenuItem key="cancel" onClick={handleChangeToCancelTransfer}>
                        <Cancel sx={{ marginRight: 1 }} /> {t(screenName + "1")}
                    </MenuItem>
                )}
                {selectedRow?.transferId && Number(user.LibraryId) === selectedRow?.sourceLibrary.sourceLibraryId && selectedRow?.transferStatus === TransferStatus.Pending && [
                    <MenuItem key="accept" onClick={handleChangeToAcceptedTransfer}>
                        <ThumbUp sx={{ marginRight: 1 }} /> {t(screenName + "2")}
                    </MenuItem>,
                    <MenuItem key="reject" onClick={handleChangeToRejectedTransfer}>
                        <ThumbDown sx={{ marginRight: 1 }} /> {t(screenName + "3")}
                    </MenuItem>
                ]}
                <Divider key={'divider'} />
                <MenuItem key={'delete'} onClick={handleDeleteClick}>
                    <Delete sx={{ marginRight: 1 }} /> {t(screenName + "4")}
                </MenuItem>
            </Menu>
            <DeleteConfirmationDialog isDialogOpen={isDeleteConfirmationDialogOpen} onDialogClose={handleDialogClose} onYesButton={handleDeleteTransfer} />
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
        </>
    )
}

export default TransfersTableMenu