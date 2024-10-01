import { Author } from "@/models/Author";
import { Book, Delete, Edit } from "@mui/icons-material";
import { Divider, Menu, MenuItem } from "@mui/material";
import { useNavigate } from "react-router-dom";
import InputNameDialog from "@/components/dialogs/InputNameDialog";
import SuccessDialog from "@/components/dialogs/SuccessDialog";
import FailureDialog from "@/components/dialogs/FailureDialog";
import { useState } from "react";
import authorService from "@/services/authorService";
import { useTranslation } from "react-i18next";

interface AuthorsTableMenuProps {
    selectedRow: Author;
    anchorEl: HTMLElement | null;
    onClose: () => void;
}

const AuthorsTableMenu: React.FC<AuthorsTableMenuProps> = ({ selectedRow, anchorEl, onClose }) => {
    const { t } = useTranslation();
    const [isSuccessDialogOpen, setSuccessDialogOpen] = useState(false);
    const [isFailureDialogOpen, setFailureDialogOpen] = useState(false);
    const [isInputNameDialogOpen, setInputNameDialogOpen] = useState(false);
    const [authorName, setAuthorName] = useState('');
    const [msg_success, setMsgSuccess] = useState("");
    const [msg_failure, setMsgFailure] = useState("");
    
    const screenName = "components.menus.authors.";
    const base_success_dialog = "components.dialogs.success.author.";
    const base_failure_dialog = "components.dialogs.failure.author.";

    const navigate = useNavigate();

    const handleEditClick = () => {
        setInputNameDialogOpen(true);
    }

    const handleShowBooksClick = () => {
        navigate(`/books?query=${selectedRow.authorName}`);
    };

    const handleDeleteClick = async () => {
        try {
            setMsgSuccess(base_success_dialog + "delete");
            setMsgFailure(base_failure_dialog + "delete");
            const response = await authorService.deleteAuthor(selectedRow.authorId);
            if (response.status === 204) {
                setSuccessDialogOpen(true);
            }
        } catch (error) {
            setFailureDialogOpen(true);
        }
    };

    const handleNameChange = (value: string) => {
        setAuthorName(value);
        return value;
    }

    const handleConfirm = async () => {
        handleDialogClose();
        
        setMsgSuccess(base_success_dialog + "create");
        setMsgFailure(base_failure_dialog + "create");

        const authorData = {
            authorId: selectedRow.authorId,
            authorName: authorName,
        }

        try {
            const response = await authorService.editAuthor(authorData);

            if (response.status === 201) {
                setSuccessDialogOpen(true);
            } else {
                setFailureDialogOpen(true);
            }

        } catch (error) {
            setFailureDialogOpen(true);
        }

        setAuthorName('');
        onClose();
    }

    const handleDialogClose = () => {
        setInputNameDialogOpen(false);
        setSuccessDialogOpen(false);
        setFailureDialogOpen(false);
    }

    return (
        <>
            <Menu
                id="rowMenu"
                anchorEl={anchorEl}
                open={Boolean(anchorEl)}
                onClose={onClose}>
                <MenuItem key="edit" onClick={handleEditClick}>
                    <Edit sx={{ marginRight: 1 }} /> {t(screenName + "1")}
                </MenuItem>
                <MenuItem key="showBooks" onClick={handleShowBooksClick}>
                    <Book sx={{ marginRight: 1 }} /> {t(screenName + "2")}
                </MenuItem>
                <Divider />
                <MenuItem key="delete" onClick={handleDeleteClick}>
                    <Delete sx={{ marginRight: 1 }} /> {t(screenName + "3")}
                </MenuItem>
            </Menu>

            {/* Input Name Dialog */}
            <InputNameDialog
                title={t(screenName + "inputNameDialog.title")}
                isDialogOpen={isInputNameDialogOpen}
                onDialogClose={handleDialogClose}
                onConfirmButton={handleConfirm}
                inputName={handleNameChange}
                initialInputValue={selectedRow?.authorName} />

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
    );
}

export default AuthorsTableMenu;