import { useEffect } from 'react';
import { useState} from 'react';
import styled from 'styled-components';
import logo from "./seeshellsLogo-flipped.png";

const howToInfo = require("./HowToInfoArray.json")

export default function Foldery ({InfoSection, currTab, testCallback, size, mobile})
{
    if (!currTab)
    {
        currTab = "default"
    }
    

    let currButton = null;

    const [selected, setSelected] = useState(currTab);
    const [thisSize, setThisSize] = useState();


    

    let obsOptions = {
        root: document.querySelector("#PageInfo"),
        threshold: .5
    }

    let shouldBold = true;

    const InfoBox = styled.div`
        flex-direction: column;
        display:flex;
        justify-content:center;
        width: ${mobile ? "100%" : "80%"};
        margin-bottom:200px;
    `
    const InfoTabBox = styled.div`
        
        display:flex;
        justify-content:center;
        width: ${mobile ? "100%" : "90%"};
        background: #2C313D;
        border-radius: 0 10px 10px 10px;
        height: fit-content;
        flex-direction: column;
    `
    const HowToTitle = styled.div`
        font-family: IBM Plex Sans Condensed;
        font-size: 30pt;
        font-weight: bold;
        margin: 2px;
        margin-bottom:5px;
    `

    const TabTitle = styled.div`
        font-family: IBM Plex Sans Condensed;
        font-size: 20pt;
        margin: 3%;
    `

    const Definition = styled.div`
        font-family: IBM Plex Sans Condensed;
        font-size: 12pt;
        margin: 3%;
    `
    const ToStatement = styled.div`
        font-family: IBM Plex Sans Condensed;
        font-size: 15pt;
        font-weight: bold;
        margin: 3%;
    `
    const StepDiv = styled.div`
        font-family: IBM Plex Sans Condensed;
        font-size: 13pt;
        margin: 3%;

    `
    const Type = styled.div`
        background: #2C313D;
        border-radius: 20px 20px 0 0;
        margin-right: 5px;
        display: flex;
        justify-content: center;
        align-items: center;
        text-align:center;
        font-size: 20px;
        font-family: "Jost";
        &:hover {
            cursor: pointer;
        };
        
    `

    const Tabs = styled.div`
        display: flex;
        flex-direction: row;
    `

    function update(Tab)
    {
        setSelected(Tab);
    }

    function checkImage(step)
    {
        if (step.image)
        {
            const image = require(`${step.image}`)
            return (
                <div style={{display:"flex", justifyContent:"center", marginTop:"10px", marginBottom:"10px", maxHeight:"85vh", maxWidth:"85vw"}}>
                    <img src={image} style={step?.imageStyles} />
                </div>
            )
        }
    }

    useEffect(() => {
        let test =  new IntersectionObserver((entries) => testCallback(InfoSection.title, entries), obsOptions)
        let element = document.getElementById(InfoSection.title)
        test.observe(element)
    }, [selected, thisSize]);

    useEffect(() => {
        setThisSize(size);
    }, [size])

    return (                          
        <InfoBox id={`${InfoSection.title}`}>
            <HowToTitle>
                {InfoSection.title}
            </HowToTitle>
            <Tabs>
                {InfoSection.tabNames.map((Tab) => {
                    return(
                        <Type key={Tab} style={{background: (InfoSection.tabs[selected].title == Tab || currTab == "default") ? "" : "#474E60", 
                        width: InfoSection.normalWidth, height: InfoSection.normalHeight}} onClick={() => update(Tab)}>
                            {Tab}
                        </Type>
                    )
                })}
            </Tabs>
            <InfoTabBox>
                <div>
          
                    <Definition>
                        {InfoSection.tabs[selected].definition }
                    </Definition>
                    <ToStatement>
                        {InfoSection.tabs[selected].toStatement}
                    </ToStatement>
                    
                    {InfoSection.tabs[selected].steps.map((Step, Index) => {
                        return (
                            <StepDiv key={`${currTab} + ${Index}`}>
                                {Step.message}
                                {checkImage(Step)}
                            </StepDiv>
                            )
                    })} 
                </div>  
            </InfoTabBox>
        </InfoBox>
    )
}