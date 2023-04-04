import styled from "styled-components";
import { useNavigate } from "react-router-dom";


export default function Header(props) 
{

  const navigation = useNavigate();

  const HeaderTab = styled.div`
    margin-right: 10px;
    font-weight: bold;
    font-size: 20px; 
    width: fit-content;
    border-bottom: ${(props.selected) ? "2px #FFFBF0 solid" : ""};
    &:hover {cursor: pointer}
  `

  function handleClick()
  {
    if (props.tab == "GitHub")
    {
        const button = document.createElement('a')
        button.href = "https://github.com/RickLeinecker/SeeShells-v3"
        button.setAttribute("target", "_blank")
        button.click()
        button.remove()
    }
    else
    {
      navigation(`/${(props.tab === "About") ? "" : props.tab.replaceAll(" ", "")}`)
    }
  }
  return (
    <HeaderTab onClick={() => handleClick()}>
        {props.tab}
    </HeaderTab>
  );
}