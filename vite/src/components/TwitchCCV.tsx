import { Box } from "@mui/material";
import type RaidUserProps from "./RaidUserProps";

/**
 * Provides the CCV count and SVG icon for a user
 * @param props - RaidUserProps
 * @returns - The SVG + CCV count in a component
 */
export default function TwitchCCV(props: RaidUserProps){
    const { isOnline, viewCount } = props;
    return(
        isOnline ? 
        <Box
            sx={{
                display: "flex",
                alignItems: "center"
            }}
        >
            <svg width="24" height="24" viewBox="0 0 24 24" aria-hidden="true">
            <path 
                fill="#ff8280"
                fill-rule="evenodd" 
                d="M6 8a6 6 0 1 1 7.025 5.913l.012.036A3 3 0 0 0 15.883 16H17a4 4 0 0 1 4 4v2h-2v-2a2 2 0 0 0-2-2h-1.117A5 5 0 0 1 12 16.15 5 5 0 0 1 8.117 18H7a2 2 0 0 0-2 2v2H3v-2a4 4 0 0 1 4-4h1.117a3 3 0 0 0 2.846-2.051l.012-.036A6.002 6.002 0 0 1 6 8Zm6 4a4 4 0 1 1 0-8 4 4 0 0 1 0 8Z" 
                clip-rule="evenodd"
            />
            </svg>
            <b style={{color:"#ff8280", paddingLeft: "5px"}}>{viewCount}</b>
        </Box>
        :
        <Box
            sx={{
                display: "flex",
                alignItems: "center"
            }}
        >
            <svg width="24" height="24" viewBox="0 0 24 24" aria-hidden="true">
            <path 
                fill="#9a9a9a"
                fill-rule="evenodd" 
                d="M6 8a6 6 0 1 1 7.025 5.913l.012.036A3 3 0 0 0 15.883 16H17a4 4 0 0 1 4 4v2h-2v-2a2 2 0 0 0-2-2h-1.117A5 5 0 0 1 12 16.15 5 5 0 0 1 8.117 18H7a2 2 0 0 0-2 2v2H3v-2a4 4 0 0 1 4-4h1.117a3 3 0 0 0 2.846-2.051l.012-.036A6.002 6.002 0 0 1 6 8Zm6 4a4 4 0 1 1 0-8 4 4 0 0 1 0 8Z" 
                clip-rule="evenodd"
            />
            </svg>
            <b style={{color:"#9a9a9a", paddingLeft: "5px"}}>0</b>
        </Box>
    )
}