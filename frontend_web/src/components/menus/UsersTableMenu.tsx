import { Assignment, Gavel } from "@mui/icons-material";
import { Divider, Menu, MenuItem } from "@mui/material";
import { useNavigate } from "react-router-dom";
import { User } from "@/models/User";
import { useTranslation } from "react-i18next";
import { useAuth } from "@/context/AuthContext";
import { useState } from "react";
import userService from "@/services/userService";
import SuccessDialog from "../dialogs/SuccessDialog";
import FailureDialog from "../dialogs/FailureDialog";

interface UsersTableMenuProps {
    selectedRow: User;
    anchorEl: HTMLElement | null;
    onClose: () => void;
}

const UsersTableMenu: React.FC<UsersTableMenuProps> = ({ selectedRow, anchorEl, onClose }) => {
    const { t } = useTranslation();
    const { user } = useAuth();
    const [isSuccessDialogOpen, setSuccessDialogOpen] = useState(false);
    const [isFailureDialogOpen, setFailureDialogOpen] = useState(false);
    const [msg_success, setMsgSucess] = useState("");
    const [msg_failure, setMsgFailure] = useState("");

    const screenName = "components.menus.users.";

    const navigate = useNavigate();

    const handleShowRequests = () => {
        navigate(`/users/requests?userId=${selectedRow.userId}`);
    };

    const handleShowPunishments = () => {
        navigate(`/users/punishments?userId=${selectedRow.userId}`);
    };
    const handleChangeRoleToLibrarian = async () => {
        try {
            const response = await userService.changeRoleToLibrarian(selectedRow.userId);

            if (response.status === 201) {
                setMsgSucess("components.dialogs.success.user.change_role")
                setSuccessDialogOpen(true);
            } else {
                setMsgFailure("components.dialogs.failure.user.change_role")
                setFailureDialogOpen(true);
            }
        } catch (error) {
            setFailureDialogOpen(true);
            setMsgFailure("components.dialogs.failure.user.change_role")
            console.error(error);
        }
        onClose();
    };

    const handleDialogClose = () => {
        setSuccessDialogOpen(false);
        setFailureDialogOpen(false);
    };

    return (
        <>
            <Menu
                id="rowMenu"
                anchorEl={anchorEl}
                open={Boolean(anchorEl)}
                onClose={onClose} >
                <MenuItem onClick={handleShowRequests}>
                    <Assignment sx={{ marginRight: 1 }} /> {t(screenName + "1")}
                </MenuItem>
                <MenuItem onClick={handleShowPunishments}>
                    <Gavel sx={{ marginRight: 1 }} /> {t(screenName + "2")}
                </MenuItem>
                {user && user["http://schemas.microsoft.com/ws/2008/06/identity/claims/role"] === "Admin" && (
                    <>
                        <Divider />
                        <MenuItem onClick={handleChangeRoleToLibrarian}>
                            <Gavel sx={{ marginRight: 1 }} /> {t(screenName + "3")}
                        </MenuItem>
                    </>
                )}
            </Menu>
            {/* Render SuccessDialog only when isSuccessDialogOpen is true */}
            {isSuccessDialogOpen && (
                <SuccessDialog isDialogOpen={isSuccessDialogOpen} onDialogClose={handleDialogClose} msg={t(msg_success)} />
            )}
            {/* Render FailureDialog only when isFailureDialogOpen is true */}
            {isFailureDialogOpen && (
                <FailureDialog isDialogOpen={isFailureDialogOpen} onDialogClose={handleDialogClose} msg={t(msg_failure)} />
            )}
        </>
    );
};

export default UsersTableMenu;