import styled from "styled-components"
import {useState, useEffect} from "react"

const options = require("./HowToUseArray.json")

export default function SideBar ({scroll, update, button, mobile})
{
    const OptionButtons = styled.div`
        text-align:left;
        font-size: 15pt;
        width: fit-content;
        margin-left: 10%; 
        margin-bottom: 15px;
        &:hover {
            cursor: pointer;
        };
    `

    const SideBar = styled.div`
        flex-direction:column;
        display:flex;
        width: 15%;
        min-width:200px;
        background: #2C313D;
        height:fit-content;
    `
    useEffect(() => {
        update(true);
    }, [button])
    console.log("Testing Side" + mobile);
    if (!mobile)
    {
        return (
            <SideBar>
                {options.options.map((option) => {
                    return(
                        <OptionButtons id = {`${option + "button"}`} style={{fontWeight : `${button == option ? "700" : ""}`}} onClick={() => scroll(option)}>
                            {option}
                        </OptionButtons>
                )})
                }
            </SideBar>
        )
    }
}