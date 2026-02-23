import React from 'react';
import { useEffect, useState } from 'react';
import './App.css'
import './pages/Settings'
import { Box, Tab, Tabs } from '@mui/material'
import StreamersPage from './pages/StreamersPage';
import useWebSocket from 'react-use-websocket';

const RAID_WS_URL_LOGIC = 'ws://192.168.2.18:10004/logic'
const RAID_WS_URL_UI = 'ws://192.168.2.18:10005/ui'

interface TabPanelProps {
	children?: React.ReactNode;
	index: number;
	value: number;
}

function TabPanel(props: TabPanelProps){
	const { children, value, index, ...other } = props;

	return (
		<div
			role="tabpanel"
			hidden={value !== index}
			id={`simple-tabpanel-${index}`}
			aria-labelledby={`simple-tab-${index}`}
			{...other}
		>
			{value === index && <Box sx={{ p: 3 }}>{children}</Box>}
		</div>
	);
}

function tabProps(ind: number){
	return {
		id: `tab-${ind}`,
		'aria-controls': `tabpenl-${ind}`
	};
}

export default function App() {
	const [value, setValue] = React.useState(0);
  const [raidTargets, setRaidTargets] = React.useState([]);

	const handleChange = (_event: React.SyntheticEvent, newValue: number) => {
		setValue(newValue);
	};

	// Establish two web socket connections; 1 from Streamer.Bot, 1 to Streamer.bot
	const {lastMessage} = useWebSocket(RAID_WS_URL_LOGIC, {
			onOpen: () => { 
					console.log('Streamer.Bot -> UI :: Connection established.'); 
			}, 
			share: true
	});


	const {sendMessage} = useWebSocket(RAID_WS_URL_UI, {
			onOpen: () => { 
					console.log('UI -> Streamer.Bot :: Connection established.'); 
					// Get raid targets on open
					// TODO
					// requestraiders()
					//console.log('Requesting raid targets to render...');
          sendMessage("test");
			}, 
			share: true
	});

/*	 function requestRaiders(){
			const json = {
					"action": "render"
			}
			sendMessage(JSON.stringify(json));
	} */

	/*
			Handler for any messages that arrive from Streamer.bot
	*/
	 useEffect(()=>{
			if(lastMessage != null){
					//console.log(lastMessage.data);
					const jsonData = JSON.parse(lastMessage.data)
					console.log(jsonData);
          setRaidTargets(jsonData.raidTargets);
			}
	},[lastMessage]);


	return (
		<>
			<Tabs 
				value={value} 
				onChange={handleChange}
				sx={{
					'& .MuiTabs-indicator':{
						backgroundColor: '#5cabff'
					},
				}}
			>
				<Tab className="tab" label="Streamers" {...tabProps(0)} />
				<Tab className="tab" label="Other" {...tabProps(0)} />
				<Tab className="tab" label="Favorites" {...tabProps(0)} />
				<Tab className="tab" label="Blacklist" {...tabProps(0)} />
				<Tab className="tab" label="Settings" {...tabProps(0)} />
			</Tabs>
			<TabPanel value={value} index={0}>
				<StreamersPage data={raidTargets}/>
			</TabPanel>
			<TabPanel value={value} index={1}>
				<h1>Other panel</h1>
			</TabPanel>
			<TabPanel value={value} index={2}>
				<h1>Other panel</h1>
			</TabPanel>
			<TabPanel value={value} index={3}>
				<h1>Other panel</h1>
			</TabPanel>
			<TabPanel value={value} index={4}>
				<h1>Other panel</h1>
			</TabPanel>
		</>
	)
}
