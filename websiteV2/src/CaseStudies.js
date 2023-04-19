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
        height: ${mobile ? "40vh" : "40vh"};
        width: ${mobile ? "60vw" : "45vw"};
        max-height: 400px;
        max-width: 450px;
        min-width: 300px;
        background: #2C313D;
        border-radius: 10px;
    `
    const CaseTitle = styled.div`
        font-family: "IBM Plex Sans Condensed";
        font-size: 18pt;
        font-weight: semibold;
        text-align: center;
        margin-bottom: 15px;
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
        &:hover {
            cursor: pointer;
        };
    `
    const Image = styled.img`
    height: ${mobile ? "250px" : "300px"};
    width: ${mobile ? "300px" : "450px"};
    max-height: 400px;
    max-width: 450px;
    min-width: 300px;
    opacity: .15;
    `
    const Logo = styled.img`
    height: ${mobile ? "40vh" : "40vh"};
    width: ${mobile ? "60vw" : "45vw"};
    max-height: 400px;
    max-width: 450px;
    min-width: 300px;
    background: #2C313D;
    border-radius: 10px;
        
    `

    const Study = styled.div`
    height: ${mobile ? "250px" : "300px"};
    width: ${mobile ? "300px" : "450px"};
    max-height: 400px;
    max-width: 450px;
    min-width: 300px;
    border-radius: 10px;
    background:#000;
    position: relative;
    &:hover {
        cursor: pointer;
    };
    `

    const StudyCaption = styled.div`
        width: 100%;
        display: flex;
        justify-content: center;
        align-items: center;
        font-size: 20px;
        margin-top:15%;
    `
    const ImageWrapper = styled.div`
       height: ${mobile ? "250px" : "300px"};
        width: ${mobile ? "300px" : "450px"};
        max-height: 400px;
        max-width: 450px;
        min-width: 300px;
        position: absolute;
    `
    const AbsWrapper = styled.div`
        position: absolute;
        display: flex;
        flex-direction: column;
        align-content:center;
        justify-content:center;
        height:100%;
        width:100%;
    `
    const StudyHeader = styled.div`
        width: 100%;
        font-weight: 600;
        font-size: 30px;
        display:flex;
        justify-content:center;
        margin-top: -15%;
    `

function displayImageOrDefault(imgString, vidLink) {
    if (imgString == null || vidLink == null) {
        return <Logo src={logo} />
    }

    let image = require(`${imgString}`)

    return (
        <Image src={image} />
    )
}

    function downloadFile(regFile, num)
    {
        const button = document.createElement('a')
        button.href = require(`${regFile}`)
        button.setAttribute("download", `CaseStudy${num}.dat`)
        button.click()
        button.remove()
    }

    function buttons(regFile, num)
    {
        if (!mobile)
        {
            return(
                <Grid item align="center" xs={7} sm={4} lg={3}>
                    <StudiesButtons onClick={() => downloadFile(regFile, num)}>
                        Reg File
                    </StudiesButtons>
                </Grid>
            )
        }
    }

    function openPdf(path)
    {
        if (path == null)
        {
            return;
        }
        else
        {
            const pdf = require(`${path}`)

            window.open(pdf);
        }
    }

    function openVideo(vidLink)
    {
        console.log(vidLink)
        const button = document.createElement('a')
        button.href = `${vidLink}`
        button.setAttribute("target", "_blank")
        button.click()
        button.remove()
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
                            <Grid  item align="center" sm={14} md={7} xl={5}>
                                <CaseTitle>
                                    {Case.title}
                                </CaseTitle>
                                <Study  onClick={() => openVideo(Case.vidLink)}>
                                    <ImageWrapper>
                                        <Image src={logo}/>
                                    </ImageWrapper>

                                    <AbsWrapper>
                                        <StudyHeader>
                                            {Case.title}
                                        </StudyHeader>
                                        <StudyCaption>
                                            Click here for a video walkthrough
                                        </StudyCaption>
                                    </AbsWrapper>

                                </Study>
                                <Caption>
                                    {Case.caption }
                                </Caption>
                                <div style={{display:"flex", flexDirection:"row", justifyContent:"center"}}>
                                    <Grid item align="center" xs={7} sm={5} lg={3} >
                                        <StudiesButtons onClick={() => openPdf(Case.pdfFile)}>
                                            PDF
                                        </StudiesButtons>
                                    </Grid>
                                    {buttons(Case.regFiles, Case.num)}
                                </div>
                            </Grid> 
                        )
                    })}
                </Grid>
                <SectionHeader>
                    External Threats
                </SectionHeader>
                <Grid container style={{justifyContent:"center", paddingBottom:"25px"}} columns={14}>
                {caseStudies.outside.map((Case) => {
                        return(
                            <Grid  item align="center" xs={7} xl={5}>
                                <CaseTitle>
                                    {Case.title}
                                </CaseTitle>
                                <Study>
                                    <ImageWrapper>
                                        <Image src={logo} />
                                    </ImageWrapper>

                                    <AbsWrapper>
                                        <StudyHeader>
                                            {Case.title}
                                        </StudyHeader>
                                        <StudyCaption>
                                            Click here for a video walkthrough
                                        </StudyCaption>
                                    </AbsWrapper>

                                </Study>
                                <Caption>
                                    {Case.caption }
                                </Caption>
                                <div style={{display:"flex", flexDirection:"row", justifyContent:"center"}}>
                                    <Grid item align="center" xs={7} sm={5} lg={3} >
                                        <StudiesButtons onClick={() => openPdf(Case.pdfFile)}>
                                            PDF
                                        </StudiesButtons>
                                    </Grid>
                                    {buttons(Case.regFiles, Case.num)}
                                </div>
                            </Grid> 
                        )
                    })}
                </Grid>
            </MainContent>
        </Contain>
        );
}