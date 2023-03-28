import styled from "styled-components"

export default function TestButton({option, onClick})
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

    return (
        <OptionButtons id = {`${option + "button"}`} onClick={onClick} style={{width:"fit-content" , marginLeft: "10%", marginBottom: "15pt"}}>
            {option}
        </OptionButtons>

    )   
}