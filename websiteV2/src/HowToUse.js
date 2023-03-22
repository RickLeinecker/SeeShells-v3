import styled from "styled-components";
import Header from "./Header";
import Foldery from "./Foldery.js";
import TestButton from "./TestButton.js"
import SideBar from "./SideBar.js"
import { MainContent, Title,  Contain } from "./customStyles";
import { useState} from "react";
import {FaListUl} from "react-icons/fa";
import {Menu} from '@mui/material';


const howToInfo = require("./HowToInfoArray.json")
const options = require("./HowToUseArray.json")




export default function HowToUse({size})
{
    let click = false;
    let button = null;

    let mobile = (size.width <= 750)

    const [finished, setFinished] = useState(false);
    const [menuOpen, setMenuOpen] = useState(false);

    const options = require("./HowToUseArray.json")

    const optionsScroll = (option) =>
    {

      

        document.getElementById(option).scrollIntoView({behavior:"smooth", alignToTop:"true"})
        
        
        if (button != null)
            button.style.fontWeight = ""
        
        button = document.getElementById(`${option + "button"}`)
        button.style.fontWeight = "700"

                        
    }

    const testCallback =  (option, entries) => {
        
        if (!mobile)
        {
            if (entries[0].intersectionRatio < 1)
            {
                return
            }
            if (click)
                return

            if (button != null)
                button.style.fontWeight = "";
            
            button = document.getElementById(`${option + "button"}`)
            button.style.fontWeight = "700"
        }
        else
        {
            if (entries[0].intersectionRatio < 1)
            {
                return
            }
            if (click)
                return

            
        }
    }

    const HeaderContent = styled.div`
        position: relative;
        background: #2C313D;
        display: flex;
        justify-content: center;
    `

    const MenuSelectBox = styled.div`
        
        display:flex;
        align-content: center;
    `
    const PageInfo = styled.div`
        flex-direction: column;
        display:flex;
        width: ${mobile ? "100vw" : "80vw"};
        height: 100%;
        align-items: center;
        margin: 2%;
        margin-bottom: 0;
        margin-top:0;
        overflow: auto;
        
    `

    const MenuButton = styled.div`

    `
    const MenuBox = styled.div`
        width: 40vw
    `

    const pageContent = () => {
        if (finished)
        {
            return(
                <PageInfo id={"PageInfo"}>
                    <Foldery InfoSection={howToInfo.parsing} testCallback={testCallback} currTab={"Online"} size ={size} mobile={mobile}/>
                        <div style={{marginBottom:"500px"}} />
                    <Foldery InfoSection={howToInfo.ShellInspector} testCallback={testCallback} currTab={"Online"} size={size} mobile={mobile} />
                </PageInfo>
            )
        }
    }
    
    function openMenu()
    {
        setMenuOpen(true);
    }


    return (
            <Contain>
                <Header tab="How to Use" size={size}/>
                <HeaderContent>
                    <Title>
                        How To Use
                    </Title>
                </HeaderContent>
                <MainContent style={{display:"flex", flexDirection:"row", overflow:"hidden"}}>
                    <MenuSelectBox>
                    <SideBar scroll={optionsScroll} update={setFinished} button={button} mobile = {mobile}/>
                            {pageContent()}
                    </MenuSelectBox>
                </MainContent>
            </Contain>
    );
}