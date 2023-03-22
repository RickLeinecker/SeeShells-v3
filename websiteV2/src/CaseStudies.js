import styled from "styled-components";
import Header from "./Header";
import logo from "./seeshellsLogo-flipped.png";
import { MainContent, Title, MainText, Contain } from "./customStyles";
import Grid from '@mui/material/Grid';

const caseStudies = require("./CaseStudiesArray.json");

export default function CaseStudies({size}) {

    let mobile = (size.width <= 750)

    const HeaderContent = styled.div`
        background: #2C313D;
    `
    const SectionHeader = styled.div`
        font-family: "IBM Plex Sans Condensed";
        font-size: 20pt;
        font-weight: bold;
        margin: 3%;
        text-align: center;
    `
    const ImageDiv = styled.div`
        margin-top: 15px;
        height: ${mobile ? "125px" : "210px"};
        width: ${mobile ? "125px" : "210px"};
        background: #2C313D;
        border-radius: 10px;
    `
    const CaseTitle = styled.div`
        font-family: "IBM Plex Sans Condensed";
        font-size: 18pt;
        font-weight: semibold;
        text-align: center;
    `
    const Caption = styled.div`
        font-family: "IBM Plex Sans Condensed";
        font-size: 13pt;
        margin-top:5px;
        text-align: center;
    `
    const StudiesButtons = styled.div`
        font-family: "IBM Plex Sans Condensed";
        font-size: ${mobile ? "12pt" : "16pt"};
        margin-top:5px;
        text-align: center;
        background: #2C313D;
        margin-top:10px;
        padding:15px;
        width: ${mobile ? "55px" : "75px"};
        border-radius: 5px;
    `
    const Image = styled.img`
        height: ${mobile ? "125px" : "210px"};
        width: ${mobile ? "125px" : "210px"};
    `
    const Logo = styled.img`
        height: ${mobile ? "125px" : "210px"};
        width: ${mobile ? "125px" : "210px"};
        

    `

    function displayImageOrDefault(imgString) {
        if (imgString == null) {
            return <Logo src={logo} />
        }

        return (
            <Image src={imgString} />
        )
    }

    function buttons()
    {
        if (!mobile)
        {
            return(
                <Grid item align="center" xs={7} sm={4} lg={3} >
                    <StudiesButtons>
                        Reg File
                    </StudiesButtons>
                </Grid>
            )
        }
    }

    return (
        <Contain>    
            <Header tab="Case Studies" size = {size}/>
            <HeaderContent>
                <Title>
                    Case Studies
                </Title>
            </HeaderContent>
            <MainContent>
                <SectionHeader>
                    Insider Threats
                </SectionHeader>
                <Grid container style={{justifyContent:"center"}} columns={14}>
                {caseStudies.inside.map((Case) => {
                        return(
                            <Grid  item align="center" xs={7} xl={5}>
                                <CaseTitle>
                                    {Case.title}
                                </CaseTitle>
                                    <ImageDiv>
                                        {displayImageOrDefault(Case.vidLink)}
                                    </ImageDiv>
                                <Caption>
                                    {Case.caption }
                                </Caption>
                                <div style={{display:"flex", flexDirection:"row", justifyContent:"center"}}>
                                    <Grid item align="center" xs={7} sm={5} lg={3} >
                                        <StudiesButtons>
                                            PDF
                                        </StudiesButtons>
                                    </Grid>
                                    {buttons()}
                                </div>
                            </Grid> 
                        )
                    })}
                </Grid>
                <SectionHeader>
                    External Threats
                </SectionHeader>
                <Grid container style={{justifyContent:"center", marginBottom:"25px"}} columns={14}>
                {caseStudies.outside.map((Case) => {
                        return(
                            <Grid  item align="center" xs={7} xl={5}>
                                <CaseTitle>
                                    {Case.title}
                                </CaseTitle>
                                    <ImageDiv>
                                    {displayImageOrDefault(Case.vidLink)}
                                </ImageDiv>
                                <Caption>
                                    {Case.caption }
                                </Caption>
                                <div style={{display:"flex", flexDirection:"row", justifyContent:"center"}}>
                                    <Grid item align="center" xs={7} sm={5} lg={3} >
                                        <StudiesButtons>
                                            PDF
                                        </StudiesButtons>
                                    </Grid>
                                    {buttons()}
                                </div>
                            </Grid> 
                        )
                    })}
                </Grid>
            </MainContent>
        </Contain>
        );
}