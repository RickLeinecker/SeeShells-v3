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
  return (
    <HeaderTab onClick={() => {navigation(`/${(props.tab === "About") ? "" : props.tab.replaceAll(" ", "")}`)}}>
        {props.tab}
    </HeaderTab>
  );
}