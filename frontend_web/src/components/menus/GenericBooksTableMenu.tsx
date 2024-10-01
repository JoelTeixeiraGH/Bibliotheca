import { Book, Edit, Delete, LibraryBooks, Visibility, Reviews } from "@mui/icons-material";
import { Divider, Menu, MenuItem } from "@mui/material";
import { useNavigate } from "react-router-dom";
import SuccessDialog from "@/components/dialogs/SuccessDialog";
import FailureDialog from "@/components/dialogs/FailureDialog";
import { useState } from "react";
import { GenericBook } from "@/models/GenericBook";
import { useTranslation } from "react-i18next";

interface GenericBooksTableMenuProps {
    selectedRow: GenericBook;
    anchorEl: HTMLElement | null;
    onClose: () => void;
}

const GenericBooksTableMenu: React.FC<GenericBooksTableMenuProps> = ({ selectedRow, anchorEl, onClose }) => {
    const { t } = useTranslation();
    const [isSuccessDialogOpen, setSuccessDialogOpen] = useState(false);
    const [isFailureDialogOpen, setFailureDialogOpen] = useState(false);

    const screenName = "components.menus.genericBooks.";
    const msg_success = "components.dialogs.success.generic_book";
    const msg_failure = "components.dialogs.failure.generic_book"

    const navigate = useNavigate();

    const handleViewClick = () => {
        navigate(`/books/view?isbn=${selectedRow?.isbn}`);
    };
    const handleViewAddCopiesClick = () => {
        navigate(`/books/physical-books?isbn=${selectedRow.isbn}`);
    };
    const handleEditClick = () => {
        navigate(`/books/edit?isbn=${selectedRow.isbn}`);
    };
    const handleDeleteClick = () => {
        //navigate(`/books/delete?isbn=${selectedRow.isbn}`);
    };

    const handleDialogClose = () => {
        setSuccessDialogOpen(false);
        setFailureDialogOpen(false);
    }

    return (
        <>
            <Menu
                id="genericBooksMenu"
                anchorEl={anchorEl}
                open={Boolean(anchorEl)}
                onClose={onClose}>
                <MenuItem key="view" onClick={handleViewClick}>
                    <Visibility sx={{ marginRight: 1 }} /> {t(screenName + "1")}
                </MenuItem>
                <MenuItem key="view/addCopies" onClick={handleViewAddCopiesClick}>
                    <LibraryBooks sx={{ marginRight: 1 }} /> {t(screenName + "2")}
                </MenuItem>
                <MenuItem key="edit" onClick={handleEditClick}>
                    <Edit sx={{ marginRight: 1 }} /> {t(screenName + "3")}
                </MenuItem>
                <Divider />
                <MenuItem key="delete" onClick={handleDeleteClick}>
                    <Delete sx={{ marginRight: 1 }} /> {t(screenName + "5")}
                </MenuItem>
            </Menu>

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

export default GenericBooksTableMenu;