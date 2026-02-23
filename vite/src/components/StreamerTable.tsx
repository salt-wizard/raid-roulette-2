import { Paper, Table, TableBody, TableCell, TableContainer, TableHead, TableRow } from "@mui/material";
import StreamerTableRow from "./StreamerTableRow";

type StreamerTableProps = {
    showOffline: boolean
    data: any //JSON
}

export default function StreamerTable({showOffline, data}: StreamerTableProps){
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
                <TableHead>
                    <TableRow>
                        <TableCell sx={{backgroundColor: "#0e0e10"}}><b style={{color:"rgba(255, 255, 255, 0.87)"}}></b></TableCell>
                        <TableCell sx={{backgroundColor: "#0e0e10"}}><b style={{color:"rgba(255, 255, 255, 0.87)"}}></b></TableCell>
                        <TableCell sx={{backgroundColor: "#0e0e10"}}><b style={{color:"rgba(255, 255, 255, 0.87)"}}>Name</b></TableCell>
                        <TableCell sx={{backgroundColor: "#0e0e10"}}><b style={{color:"rgba(255, 255, 255, 0.87)"}}>Playing</b></TableCell>
                        <TableCell sx={{backgroundColor: "#0e0e10"}}><b style={{color:"rgba(255, 255, 255, 0.87)"}}>Chat Restrictions</b></TableCell>
                        <TableCell sx={{backgroundColor: "#0e0e10"}}><b style={{color:"rgba(255, 255, 255, 0.87)"}}>Remove</b></TableCell>
                        <TableCell sx={{backgroundColor: "#0e0e10"}}><b style={{color:"rgba(255, 255, 255, 0.87)"}}>Details</b></TableCell>
                    </TableRow>
                </TableHead>
                <TableBody>
                    {data.map((e:any) => (
                        <StreamerTableRow 
                            {...e}
                            showOffline={showOffline}
                            key={e.userId}
                        /> 
                    ))}
                </TableBody>
            </Table>
            </TableContainer>
        </>
    )
}