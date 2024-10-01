import { Divider, Menu, MenuItem } from '@mui/material';
import { Cancel, Start, Done, MoreTime, Delete } from '@mui/icons-material';
import React, { useState } from 'react';
import DeleteConfirmationDialog from '@/components/dialogs/DeleteConfirmationDialog';
import InputDateDialog from '@/components/dialogs/InputDateDialog';
import requestService from '@/services/requestService';
import SuccessDialog from '@/components/dialogs/SuccessDialog';
import FailureDialog from '@/components/dialogs/FailureDialog';
import { Request, RequestStatus } from '@/models/Request';
import { useTranslation } from 'react-i18next';

interface RequestsTableMenuProps {
    selectedRow: Request;
    anchorEl: HTMLElement | null;
    onClose: () => void;
}

const RequestsTableMenu: React.FC<RequestsTableMenuProps> = ({ selectedRow, anchorEl, onClose }) => {
    const { t } = useTranslation();
    const [isSuccessDialogOpen, setSuccessDialogOpen] = useState(false);
    const [isFailureDialogOpen, setFailureDialogOpen] = useState(false);
    const [isDeleteConfirmationDialogOpen, setDeleteConfirmationDialogOpen] = useState(false);
    const [isInputDateDialogOpen, setInputDateDialogOpen] = useState(false);
    const [date, setDate] = useState('');
    const [currentOption, setCurrentOption] = useState(0);
    const [msg_success, setMsgSucess] = useState("");
    const [msg_failure, setMsgFailure] = useState("");

    const screenName = "components.menus.requests.";


    const handleDeleteRequest = async () => {
        setDeleteConfirmationDialogOpen(false);

        try {
            const response = await requestService.deleteRequest(selectedRow.requestId);

            if (response.status === 201) {
                setMsgSucess("components.dialogs.success.requests.delete");
                setSuccessDialogOpen(true);
            } else {
                setMsgFailure("components.dialogs.failure.requests.delete");
                setFailureDialogOpen(true);
            }

        } catch (error) {
            setMsgFailure("components.dialogs.failure.requests.delete");
            setFailureDialogOpen(true);
        }

        onClose();
    };

    const handleCancelRequest = async () => {
        try {
            const response = await requestService.changeRequestStatusToCanceled(selectedRow.requestId);

            if (response.status === 201) {
                setMsgSucess("components.dialogs.success.requests.cancel");
                setSuccessDialogOpen(true);
            } else {
                setMsgFailure("components.dialogs.failure.requests.cancel")
                setFailureDialogOpen(true);
            }

        } catch (error) {
            setMsgFailure("components.dialogs.failure.requests.cancel")
            setFailureDialogOpen(true);
        }

        onClose();
    };


    const handleChangeToReturnedRequest = async () => {
        try {
            const response = await requestService.changeRequestStatusToReturned(selectedRow.requestId);

            if (response.status === 201) {
                setMsgSucess("components.dialogs.success.requests.returned");
                setSuccessDialogOpen(true);
            } else {
                setMsgFailure("components.dialogs.failure.requests.returned");
                setFailureDialogOpen(true);
            }

        } catch (error) {
            setMsgFailure("components.dialogs.failure.requests.returned");
            setFailureDialogOpen(true);
        }

        onClose();
    };

    const handleExtendTimeRequest = async () => {
        setInputDateDialogOpen(false);

        try {
            const response = await requestService.extendTime(selectedRow.requestId, date);

            if (response.status === 201) {
                setMsgSucess("components.dialogs.success.requests.extend_time")
                setSuccessDialogOpen(true);
            } else {
                setMsgFailure("components.dialogs.failure.requests.extend_time");
                setFailureDialogOpen(true);
            }

        } catch (error) {
            setMsgFailure("components.dialogs.failure.requests.extend_time");
            setFailureDialogOpen(true);
        }

        setDate('');
        onClose();
    };

    const handleChangeToRequestedRequest = async () => {
        setInputDateDialogOpen(false);

        try {
            const response = await requestService.changeRequestStatusToRequested(selectedRow.requestId, date);

            if (response.status === 201) {
                setMsgSucess("components.dialogs.success.requests.requested")
                setSuccessDialogOpen(true);
            } else {
                setMsgFailure("components.dialogs.failure.requests.requested");
                setFailureDialogOpen(true);
            }

        } catch (error) {
            setMsgFailure("components.dialogs.failure.requests.requested");
            setFailureDialogOpen(true);
        }

        setDate('');
        onClose();
    };

    const handleDeleteClick = () => {
        setDeleteConfirmationDialogOpen(true);
    };

    const handleExtendTimeClick = () => {
        setCurrentOption(1);
        setInputDateDialogOpen(true);
    };

    const handleChangeToRequestedClick = () => {
        setCurrentOption(2);
        setInputDateDialogOpen(true);
    };

    const handleDialogClose = () => {
        setCurrentOption(0);
        setSuccessDialogOpen(false);
        setFailureDialogOpen(false);
        setDeleteConfirmationDialogOpen(false);
        setInputDateDialogOpen(false);
        onClose();
    };

    const handleDateChange = (value: string) => {
        setDate(value);
        console.log(value)
        return value;
    };

    const handleConfirm = () => {
        if (currentOption === 1) {
            handleExtendTimeRequest();
        } else {
            handleChangeToRequestedRequest();
        }
    };

    return (
        <>
            <Menu
                id="rowMenu"
                anchorEl={anchorEl}
                open={Boolean(anchorEl)}
                onClose={onClose} >
                {selectedRow?.requestId && (selectedRow.requestStatus === RequestStatus.Pending || selectedRow.requestStatus === RequestStatus.Waiting) && [
                    <MenuItem key={'cancel'} onClick={handleCancelRequest}>
                        <Cancel sx={{ marginRight: 1 }} /> {t(screenName + "1")}
                    </MenuItem>,
                    <MenuItem key={'requested'} onClick={handleChangeToRequestedClick}>
                        <Start sx={{ marginRight: 1 }} /> {t(screenName + "2")}
                    </MenuItem>
                ]}
                {selectedRow?.requestId && (selectedRow.requestStatus === RequestStatus.Requested || selectedRow.requestStatus === RequestStatus.NotReturned) && [
                    <MenuItem key={'returned'} onClick={handleChangeToReturnedRequest}>
                        <Done sx={{ marginRight: 1 }} /> {t(screenName + "3")}
                    </MenuItem>,
                    <MenuItem key={'extend'} onClick={handleExtendTimeClick}>
                        <MoreTime sx={{ marginRight: 1 }} /> {t(screenName + "4")}
                    </MenuItem>
                ]}
                {selectedRow?.requestId && (selectedRow.requestStatus === RequestStatus.Returned || selectedRow.requestStatus === RequestStatus.Canceled) && [
                    <Divider key={'divider'} />,
                    <MenuItem key={'delete'} onClick={handleDeleteClick}>
                        <Delete sx={{ marginRight: 1 }} /> {t(screenName + "5")}
                    </MenuItem>
                ]}
            </Menu>
            <InputDateDialog
                isDialogOpen={isInputDateDialogOpen}
                onDialogClose={handleDialogClose}
                onConfirmButton={handleConfirm}
                inputDate={handleDateChange} />
            <DeleteConfirmationDialog isDialogOpen={isDeleteConfirmationDialogOpen} onDialogClose={handleDialogClose} onYesButton={handleDeleteRequest} />
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

export default RequestsTableMenu