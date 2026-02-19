import { Avatar, Box, Collapse, IconButton, TableCell, TableRow } from "@mui/material";
import DeleteIcon from '@mui/icons-material/Delete';
import KeyboardArrowDownIcon from '@mui/icons-material/KeyboardArrowDown';
import KeyboardArrowUpIcon from '@mui/icons-material/KeyboardArrowUp';
import type RaidUserProps from "./RaidUserProps";
import TwitchCCV from "./TwitchCCV";
import React from "react";
import TwitchPartnerSVG from "./TwitchPartnerSVG";

export default function UserTableRow(props: RaidUserProps){
    const { 
        userName, 
        userPfp, 
        gameName, 
        streamTitle, 
        streamPreview,
        slowMode,
        followerMode,
        subscriberMode,
        emoteMode,
        uniqueChatMode,
    } = props;




    const [open, setOpen] = React.useState(false);

    return(
        <>
        <TableRow>
            <TableCell sx={{width: 1/100}}><TwitchCCV {...props}/></TableCell>
            <TableCell sx={{width: 1/25}}><Avatar alt={userName} src={userPfp}/></TableCell>
            <TableCell sx={{width: 1/10}}>
                <Box
                    sx={{
                        display: "flex",
                        justify: "left",
                        alignItems: "center"
                    }}
                >
                    <TwitchPartnerSVG {...props}/><b style={{color:"rgba(255, 255, 255, 0.87)"}}>{userName}</b>
                </Box>
            </TableCell>
            <TableCell sx={{width: 1/10}}><b style={{color:"rgba(255, 255, 255, 0.87)"}}>{gameName}</b></TableCell>
            <TableCell sx={{width: 1/10}}>
                    {
                        (slowMode || followerMode || subscriberMode || emoteMode || uniqueChatMode)
                        ?
                        <b style={{color:"rgba(255, 255, 255, 0.87)"}}>Yes</b>
                        :
                        <b style={{color:"rgba(255, 255, 255, 0.87)"}}>No</b>
                    }
            </TableCell>
            <TableCell sx={{width: 1/10}}><IconButton><DeleteIcon sx={{color: "white"}}/></IconButton></TableCell>
            <TableCell>
                <IconButton
                    aria-label="expand row"
                    size="small"
                    onClick={() => setOpen(!open)}
                >
                    {open ? <KeyboardArrowUpIcon sx={{color: "white"}}/> : <KeyboardArrowDownIcon sx={{color: "white"}}/>}
                </IconButton>
            </TableCell>
        </TableRow>
        <TableRow>
            <TableCell style={{ paddingBottom: 0, paddingTop: 0 }} colSpan={100}>
                <Collapse in={open} timeout="auto" unmountOnExit>
                    <Box
                        sx={{
                            display: "flex",
                            flexDirection: "column",
                            alignItems: "flex-start",
                            paddingTop: "10px",
                            paddingBottom: "5px"
                        }}
                    >
                        <b style={{color:"rgba(255, 255, 255, 0.87)"}}>{streamTitle}</b>
                        <img alt={streamTitle} src={streamPreview}></img>
                    </Box>
                </Collapse>
            </TableCell>
        </TableRow>
        </>
    )
}