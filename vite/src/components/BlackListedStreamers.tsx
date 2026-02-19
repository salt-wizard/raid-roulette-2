import { Paper, Table, TableBody, TableCell, TableContainer, TableHead } from "@mui/material";
import UserTableRow from "./UserTableRow";

const es = [{"userId":743265611,"userLogin":"late_primrose","userName":"late_Primrose","userPfp":"https://static-cdn.jtvnw.net/jtv_user_pictures/98d089d5-391c-48b3-9f0a-ae58e1007da4-profile_image-300x300.png","broadcasterType":"affiliate","isBlacklisted":false,"raidCount":0,"lastRaidDate":"","raidedByCount":0,"lastRaidedByDate":"","isOnline":false,"viewCount":0,"gameId":0,"gameName":"","streamTitle":"","streamPreview":"","tags":[],"isMature":false,"streamStart":"","slowMode":false,"followerMode":false,"subscriberMode":false,"emoteMode":false,"uniqueChatMode":false},{"userId":554416063,"userLogin":"maplewinters","userName":"maplewinters","userPfp":"https://static-cdn.jtvnw.net/jtv_user_pictures/73d95d89-1152-4d36-959c-92f55d9c2de4-profile_image-300x300.png","broadcasterType":"partner","isBlacklisted":false,"raidCount":0,"lastRaidDate":"","raidedByCount":0,"lastRaidedByDate":"","isOnline":false,"viewCount":0,"gameId":0,"gameName":"","streamTitle":"","streamPreview":"","tags":[],"isMature":false,"streamStart":"","slowMode":false,"followerMode":false,"subscriberMode":false,"emoteMode":false,"uniqueChatMode":false},{"userId":1062006219,"userLogin":"lululasso","userName":"lululasso","userPfp":"https://static-cdn.jtvnw.net/jtv_user_pictures/574e8b26-f392-4510-9ca5-dae2dbfc735f-profile_image-300x300.png","broadcasterType":"partner","isBlacklisted":false,"raidCount":0,"lastRaidDate":"","raidedByCount":0,"lastRaidedByDate":"","isOnline":false,"viewCount":0,"gameId":0,"gameName":"","streamTitle":"","streamPreview":"","tags":[],"isMature":false,"streamStart":"","slowMode":false,"followerMode":false,"subscriberMode":false,"emoteMode":false,"uniqueChatMode":false},{"userId":1157844575,"userLogin":"zfg247","userName":"Zfg247","userPfp":"https://static-cdn.jtvnw.net/jtv_user_pictures/a5030354-1f48-4fb1-a81b-6aeefc25aa69-profile_image-300x300.png","broadcasterType":"partner","isBlacklisted":false,"raidCount":0,"lastRaidDate":"","raidedByCount":0,"lastRaidedByDate":"","isOnline":true,"viewCount":46,"gameId":499973,"gameName":"Always On","streamTitle":"24/7 Zfg1 Watch Party | Ocarina of Time Ganonless speedrun testing/practice","streamPreview":"https://static-cdn.jtvnw.net/previews-ttv/live_user_zfg247-440x248.jpg","tags":["English"],"isMature":false,"streamStart":"02/18/2026 01:18:32","slowMode":false,"followerMode":true,"subscriberMode":false,"emoteMode":false,"uniqueChatMode":false},{"userId":777164349,"userLogin":"ronennhaustoria","userName":"RonennHaustoria","userPfp":"https://static-cdn.jtvnw.net/jtv_user_pictures/6a76ba98-c2c7-4fc6-a1a7-a30470f17a58-profile_image-300x300.png","broadcasterType":"partner","isBlacklisted":false,"raidCount":0,"lastRaidDate":"","raidedByCount":0,"lastRaidedByDate":"","isOnline":true,"viewCount":127,"gameId":115977,"gameName":"The Witcher 3: Wild Hunt","streamTitle":"hello contrary to recent events I am perfectly fine and everything that is not fine is in containment (also Witcher 3 prob)","streamPreview":"https://static-cdn.jtvnw.net/previews-ttv/live_user_ronennhaustoria-440x248.jpg","tags":["vtuber","OlderStreamer","English","ENVtuber","hag","SoothingVoice","wineaunt","oldlady","Bifauxnen"],"isMature":true,"streamStart":"02/19/2026 04:07:53","slowMode":false,"followerMode":false,"subscriberMode":false,"emoteMode":false,"uniqueChatMode":false}];


export default function BlackListedStreamers(){
    return(
        <>
            {/** TEXT BOX + ADD BUTTON */}
            {/** LIST OF STREAMERS */}
            <TableContainer
                component={Paper}
                sx={{ 
                    //maxHeight: 300,
                    backgroundColor: "#0e0e10"
                }}
            >
            <Table
                stickyHeader
                sx={{
                    width: 1
                }}
            >
                <TableHead
                    
                >
                    <TableCell sx={{backgroundColor: "#0e0e10"}}><b style={{color:"rgba(255, 255, 255, 0.87)"}}></b></TableCell>
                    <TableCell sx={{backgroundColor: "#0e0e10"}}><b style={{color:"rgba(255, 255, 255, 0.87)"}}></b></TableCell>
                    <TableCell sx={{backgroundColor: "#0e0e10"}}><b style={{color:"rgba(255, 255, 255, 0.87)"}}>Name</b></TableCell>
                    <TableCell sx={{backgroundColor: "#0e0e10"}}><b style={{color:"rgba(255, 255, 255, 0.87)"}}>Playing</b></TableCell>
                    <TableCell sx={{backgroundColor: "#0e0e10"}}><b style={{color:"rgba(255, 255, 255, 0.87)"}}>Chat Restrictions</b></TableCell>
                    <TableCell sx={{backgroundColor: "#0e0e10"}}><b style={{color:"rgba(255, 255, 255, 0.87)"}}>Remove</b></TableCell>
                    <TableCell sx={{backgroundColor: "#0e0e10"}}><b style={{color:"rgba(255, 255, 255, 0.87)"}}>Details</b></TableCell>
                </TableHead>
                <TableBody>
                    {es.map(e => (
                        <UserTableRow {...e} key={e.userId}/> 
                    ))}
                </TableBody>
            </Table>
            </TableContainer>
        </>
    )
}