import React from 'react';
import './App.css'
import './components/Settings'
import Settings from './components/Settings'
import { Box, Tab, Tabs } from '@mui/material'

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

  const handleChange = (_event: React.SyntheticEvent, newValue: number) => {
    setValue(newValue);
  };

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
        <Tab className="tab" label="Settings" {...tabProps(0)} />
        <Tab className="tab" label="Other" {...tabProps(0)} />
      </Tabs>
      <TabPanel value={value} index={0}>
        <Settings />
      </TabPanel>
      <TabPanel value={value} index={1}>
        <h1>Other panel</h1>
      </TabPanel>
    </>
  )
}
