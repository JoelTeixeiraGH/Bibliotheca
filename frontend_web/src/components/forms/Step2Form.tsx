import { Autocomplete, MenuItem, TextField } from '@mui/material'
import React, { useEffect, useState } from 'react'
import genericBookService from '../../services/genericBookService';
import { GenericBook } from '../../models/GenericBook';
import physicalBookService from '../../services/physicalBookService';
import { PhysicalBook } from '../../models/PhysicalBook';
import { useTranslation } from 'react-i18next';

interface Step2FormProps {
    step1Choice: object | null;
    libraryId: number;
    onGenericBookChange: (value: GenericBook | null) => void;
    isPhysicalBooksEmpty: (value: boolean) => boolean;
    onPhysicalBookChange: (value: PhysicalBook | null) => void;
}

const Step2Form: React.FC<Step2FormProps> = ({ step1Choice, libraryId, onGenericBookChange, isPhysicalBooksEmpty, onPhysicalBookChange }) => {
    const [genericBooks, setGenericBooks] = useState<GenericBook[]>([]);
    const [isGenericBooksLoading, setIsGenericBooksLoading] = useState(false);
    const [selectedGenericBook, setSelectedGenericBook] = useState<GenericBook | null>(null);
    const [physicalBooks, setPhysicalBooks] = useState<PhysicalBook[]>([]);
    const [isPhysicalBooksLoading, setIsPhysicalBooksLoading] = useState(false);
    const [selectedPhysicalBook, setSelectedPhysicalBook] = useState<PhysicalBook | null>(null);
    const [error, setError] = useState(false);
    const { t } = useTranslation();

    const screenName = "components.forms.step2.";

    useEffect(() => {
        setIsGenericBooksLoading(true);

        const fetchGenericBooks = async () => {
            try {
                const response = await genericBookService.getAllGenericBooks();

                if (response.status === 200) {
                    setGenericBooks(response.data);
                }
            } catch (error: any) {
                setError(error.message)
            }
        };

        setIsGenericBooksLoading(false);
        fetchGenericBooks();
    }, []);

    useEffect(() => {

        const fetchPhysicalBooks = async (isbn: string) => {
            try {
                const response = await physicalBookService.getAllPhysicalBooksByISBNCurrentlyAtLibrary(isbn, libraryId);

                if (response.status === 200) {
                    setPhysicalBooks(response.data);
                } else {
                    setPhysicalBooks([]);
                    isPhysicalBooksEmpty(true);
                }
            } catch (error: any) {
                setError(error.message);
            }
        };

        if (selectedGenericBook) {
            isPhysicalBooksEmpty(false);
            setSelectedPhysicalBook(null);
            setIsPhysicalBooksLoading(true);
            fetchPhysicalBooks(selectedGenericBook.isbn);
            setIsPhysicalBooksLoading(false);
        }

    }, [selectedGenericBook]);

    useEffect(() => {
        isPhysicalBooksEmpty(false);
        setSelectedPhysicalBook(null);
    }, [step1Choice]);

    // Add this function to handle physical book change
    const handlePhysicalBookChange = (event: React.ChangeEvent<{ value: unknown }>) => {
        const selectedValue = event.target.value as PhysicalBook;
        setSelectedPhysicalBook(selectedValue);
        onPhysicalBookChange(selectedValue);
    };

    return (
        <>
            {isGenericBooksLoading ? (
                <img src="src\assets\loading.svg" alt="Loading" style={{ width: '60px', height: '60px' }} />
            ) : (
                <Autocomplete
                    id="genericBook-autocomplete"
                    options={genericBooks}
                    getOptionLabel={(genericBook: GenericBook) => `${genericBook.isbn} | ${genericBook.title}`}
                    onChange={(e, selectedValue) => {
                        setSelectedGenericBook(selectedValue);
                        onGenericBookChange(selectedValue);
                    }}
                    renderInput={(params) => (
                        <TextField {...params} label={t(screenName + "label1")} fullWidth />
                    )} />
            )}
            {isPhysicalBooksLoading && selectedGenericBook ? (
                <img src="src\assets\loading.svg" alt="Loading" style={{ width: '60px', height: '60px' }} />
            ) : (
                <>
                    {physicalBooks.length > 0 && (
                        <TextField
                            label={t(screenName + "label2")}
                            value={selectedPhysicalBook || ''}
                            onChange={handlePhysicalBookChange}
                            fullWidth
                            select
                            sx={{ mt: 2 }} >
                            {physicalBooks.map((physicalBook) => {
                                return (
                                    <MenuItem key={physicalBook.physicalBookId} value={physicalBook}>
                                        {`ID Livro: ${physicalBook.physicalBookId}`}
                                    </MenuItem>
                                )
                            })}
                        </TextField>
                    )}
                </>
            )}
        </>
    );
}

export default Step2Form