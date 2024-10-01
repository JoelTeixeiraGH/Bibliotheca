import { Autocomplete, TextField } from '@mui/material';
import React from 'react';

interface Step1FormProps<T> {
  options: T[];
  getOptionLabel: (option: T) => string;
  onChange: (event: React.ChangeEvent<{}>, value: T | null) => void;
  label: string;
}

const Step1Form = <T,>({ options, getOptionLabel, onChange, label }: Step1FormProps<T>) => {
  return (
    <Autocomplete
      options={options}
      getOptionLabel={getOptionLabel}
      onChange={onChange}
      renderInput={(params) => (
        <TextField {...params} label={label} fullWidth />
      )}
    />
  );
}

export default Step1Form
