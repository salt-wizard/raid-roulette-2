import HelpOutlineIcon from '@mui/icons-material/HelpOutline';
import { Box, Tooltip } from '@mui/material';
import BlackListedStreamers from './BlackListedStreamers';

export default function Settings(){
    return (
        <>
            <h2>Blacklists</h2> 
            <Box>
                <Box sx={{display: 'flex', justifyContent: 'left', alignItems: 'center'}}>
                    <h3>Streamers</h3>
                    <Tooltip 
                        title="Raid targets to exclude."
                    >
                        <HelpOutlineIcon fontSize="small"/>    
                    </Tooltip>
                </Box>
                <BlackListedStreamers/>
            </Box>


            
            <Box sx={{display: 'flex', justifyContent: 'left', alignItems: 'center'}}>
                <h3>Users</h3>
                <Tooltip 
                    title="Suggestions from the following users will be ignored."
                >
                    <HelpOutlineIcon sx={{fontSize: 17}}/>     
                </Tooltip>
            </Box>

            
            <Box sx={{display: 'flex', justifyContent: 'left', alignItems: 'center'}}>
                <h3>Tags</h3>
                <Tooltip 
                    title="Exclude any raid target if they have any of the following tags."
                >
                    <HelpOutlineIcon fontSize="small"/>    
                </Tooltip>
            </Box>
            
            
            <Box sx={{display: 'flex', justifyContent: 'left', alignItems: 'center'}}>
                <h3>Games / Categories</h3>
                <Tooltip 
                    title="Exclude any raid target if they have any of the following games / categories."
                >
                    <HelpOutlineIcon fontSize="small"/>    
                </Tooltip>
            </Box>
        </>
    )
}