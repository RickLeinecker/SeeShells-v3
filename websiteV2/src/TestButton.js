import styled from "styled-components"

export default function testButton({option})
{
    let button = null;
    
    const OptionButtons = styled.div`
        text-align:left;
        font-size: 15pt;
        width: fit-content;
        margin-left: 10%; 
        marginBottom: 15pt;
        &:hover {
            cursor: pointer;
        };
    `
    function optionsScroll(option)
    {
        console.log(document.getElementById(option).id + " Testing")

        document.getElementById(option).scrollIntoView({behavior:"smooth", alignToTop:"true"})
        if (button != null)
            button.style.fontWeight = "";

        button = document.getElementById(`${option + "button"}`);
        console.log(button.id)
        button.style.fontWeight = "700";
        
    }

    return (
        <OptionButtons id = {`${option + "button"}`} onClick={() => optionsScroll(option)} style={{width:"fit-content" , marginLeft: "10%", marginBottom: "15pt"}}>
            {option}
        </OptionButtons>

    )   
}