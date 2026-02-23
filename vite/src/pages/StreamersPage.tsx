import HelpOutlineIcon from '@mui/icons-material/HelpOutline';
import { Box, FormControlLabel, FormGroup, Switch, Tooltip } from '@mui/material';
import StreamerTable from '../components/StreamerTable';
import React from 'react';

interface Props {
    data: any
}

export default function StreamersPage(props: Props){
    const [showOffline, setShowOffline] = React.useState(false);
    
    function handleShowOffline(){
        setShowOffline(!showOffline)
    }

    return (
        <>
            <Box>
                <FormGroup>
                    <FormControlLabel 
                        control={<Switch
                            onChange={handleShowOffline}
                        />} 
                        label="Show Offline Streamers" />
                    <FormControlLabel disabled control={<Switch />} label="Disabled" />
                </FormGroup>
            </Box>
            <Box>
                <Box sx={{display: 'flex', justifyContent: 'left', alignItems: 'center'}}>
                    <h3>Streamers</h3>
                    <Tooltip 
                        title="Raid targets to exclude."
                    >
                        <HelpOutlineIcon fontSize="small"/>    
                    </Tooltip>
                </Box>
                <StreamerTable data={props.data} showOffline={showOffline}/>
            </Box>
        </>
    )
}