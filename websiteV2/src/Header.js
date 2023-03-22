import React from "react";
import styled from "styled-components";
import HeaderTab from "./HeaderTab";
import logo from "./seeshellsLogo-flipped.png";
import {useState, useEffect} from "react";
import { useNavigate } from "react-router-dom";
import {FaBars} from "react-icons/fa";
import {Drawer} from '@mui/material';
import { RiCloseFill } from "react-icons/ri";

const tabs = require("./tabs.json")



export default function Header(props) 
{
    useEffect(() => {
        console.log("Re-rendering")
    }, [])

    const [drawer, setDrawer] = useState(false);
  
    const HeaderBar = styled.div`
        display: flex;
        width: 100vw;
        background: #2C313D;
        height: "10vh";
        color: #FFFBF0;
        font-family: "IBM Plex Sans Condensed";
  `

  const DrawerContainer = styled.div`
      height: 100%;
      width: 40vw;
      background: #2C313D;
      color: #FFFBF0;
      min-width: 200px;
  `

    const HeaderContent = styled.div`
        padding-left: .75vh;
        padding-right: .75vh;
        display: flex;
        height: 9.25vh;
        align-items: center;
        width: 100vw;
        justify-content: space-between;
    `

    const Logo = styled.div`
        margin-left: 15px;
        margin-top: 20px;
    `
    const Download = styled.div`
        background: #1C1E23;
        font-size: 20px;
        border-radius: 40px;
        width: 120px;
        margin-left: 15px;
        text-align: center;
        font-weight: bold;
        padding:5px;
        &:hover {
            cursor: pointer;
        };
    `

    const HomeButton = styled.div`
        &:hover {
            cursor: pointer;
        };
        display: flex;
    `
    const ExitButton = styled.div`
        display: flex;
        justify-content: flex-end;
        align-items: center;
        margin-right: 10px;
        margin-top: 10px
    `
    const MobileTab = styled.div`
        color: white;
        font-weight: bold;
        font-size: 30px;
        margin-top:10px;
        text-align: center
    `

    const navigation = useNavigate();

    function downloadSeeShells()
    {
        const button = document.createElement('a')
        button.href = "https://github.com/ShellBags/v2/releases/download/v2.0-beta.4/SeeShellsV2.zip"
        button.setAttribute("download", "SeeShellsV2")
        button.click()
        button.remove()
    }

    function checkSize()
    {
        if (props.size.width <= 750)
        {
            return(
                <HeaderContent>
                    <FaBars style={{width:"40px", height:"40px"}} onClick={() => setDrawer(true)} />
                    <div style={{display:"flex", flexDirection: "row", alignItems:"center"}}> 
                        <h1 style={{fontSize: 35}}>
                        SeeShells
                        </h1>
                    </div>
                </HeaderContent>
            )
        }
        else
        {
            return(
               <HeaderContent>
                    {about()}
                    <div style={{display: "flex", flexDirection: "row"}}>
                        {tabs.tabs.map((tab) =>{
                        return(
                            <HeaderTab selected = {(tab == props.tab)} tab = {tab}>
                                {tab}
                            </HeaderTab>
                        )
                        })}
                    </div>
                </HeaderContent>
            )
        }
    }

    function about()
    {
        if (props.tab != "About" && props.size.width >= 850)
        {
            return(
                <div style={{display:"flex", flexDirection: "row", alignItems:"center"}}>
                    <HomeButton onClick={() => {navigation("/")}}>
                        <h1 style={{fontSize: 35}}>
                            SeeShells
                        </h1>
                        <Logo>
                            <img src={logo} width={"50vw"} height={"50vh"}/>
                        </Logo>
                    </HomeButton>
                    <Download onClick={downloadSeeShells}>
                        Download
                    </Download>
                </div>
            )
        }
        else
        {
            return(
                <div style={{display:"flex", flexDirection: "row", alignItems:"center"}}> 
                    <h1 style={{fontSize: 35}}>
                    SeeShells
                    </h1>
                </div>
            )
        }
    }

    
      const drawerContents = () => (
          <DrawerContainer>
            <ExitButton>
                <RiCloseFill style={{color: "#FFFBF0", height:"40px", width:"40px"}} onClick={() => setDrawer(false)}/>
            </ExitButton>
            {tabs.tabs.map((tab) =>{
                return(
                    <MobileTab onClick={() => {navigation(`/${(tab == "About") ? "" : tab.replaceAll(" ", "")}`)}}>
                        {tab}
                    </MobileTab>
                )
                })}
          </DrawerContainer>
      );

  return (
    <div>
        <HeaderBar>
            {checkSize()}
        </HeaderBar>
        <Drawer anchor={'left'} onClose={() => setDrawer(false)} open={drawer} sx={{width:"40vw"}}>
            {drawerContents()}
        </Drawer>
    </div>
  );
}