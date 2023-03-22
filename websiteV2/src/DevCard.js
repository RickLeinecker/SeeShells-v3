import styled from "styled-components";
import logo from "./seeshellsLogo-flipped.png";
import {MainText} from "./customStyles";
import {Col} from "react-bootstrap";

export default function DevCard({dev})
{
    const ImageDiv = styled.div`
        margin-top: 15px;
        height: 210px;
        width: 210px;
        background: #2C313D;
        border-radius: 10px;
    `

    const Image = styled.img`
        height: 210px;
        width: 210px;
    `
    const Logo = styled.img`
        height: 190px;
        width: 190px;
        margin-top: 10px;

    `
    function displayImageOrDefault(imgString)
    {
        if (imgString == null)
        {
            return <Logo src= {logo} />
        }

        return(
            <Image src = {imgString} />
        )
    }

    function resume()
    {
        if(dev.resume != null)
        {
            return(
                dev.resume
            )
        }
    }

    return(
            <div style={{textAlign:"center", width:210, marginTop:"20px", marginRight:"15px"}}>
                <div style={{whiteSpace: "pre",height:75, justifyContent:"center", alignItems:"center", display:"flex", textAlign:"center"}}>    
                    <MainText style={{fontSize:25}}>
                        {dev.name}
                    </MainText>
                </div>
                <ImageDiv>
                    {displayImageOrDefault(dev.picture)}
                </ImageDiv>
                <div style={{marginTop:20}}>
                    <MainText style={{fontSize: 20}}>
                        {dev.role}
                        <br/>
                        {resume()}
                    </MainText>
                </div>
            </div>
    )
}