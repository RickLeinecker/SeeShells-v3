import styled from "styled-components";
import Header from "./Header";
import DevCard from "./DevCard";
import {v3devs, v2devs, v1devs} from "./DevArray";
import Grid from '@mui/material/Grid';
import { MainContent, Title, MainText, Contain} from "./customStyles";

export default function Devs({size})
{

    const HeaderContent = styled.div`
        background: #2C313D;
    `

    const Description = styled.div`
        text-align: center;
        font-size: 20px;
        margin-top: 3vh;
    `

    const Subtitle = styled.h2`
        font-family: "IBM Plex Sans Hebrew";
        font-size: 30px;
        
    `

    const Developers = styled.div`
        display: flex;
        flex-direction: column;
        padding-left: 25px;
        padding-right: 25px;
        align-items: center;
        justify-content: center;
    `

    const Sponsor = styled.div`
        font-family: "Jost";
        font-size: 20;
        padding-bottom: 10px;
        margin-top: -12px;

    `

    
    const Line = styled.div`
        border-bottom: 2px solid white;
        width: 80vw;
        text-align: center;
        height: auto;
    `
    
    return(
        <Contain>
            <Header tab = "Developers" size={size}/>
            <HeaderContent>
                    <Title>
                        Meet The Team
                    </Title>
            </HeaderContent>
            <MainContent>
                <Description>
                    <MainText>
                        SeeShells has been created through the efforts of three separate teams of five students as <br/>
                        a sponsored project for Senior Design; a year-long capstone course for Computer Science majors. 
                    </MainText>
                </Description>
                <Developers>
                    <Line>
                        <Subtitle>
                            SeeShells V3
                        </Subtitle>
                        <Sponsor>
                            Sponsor: Professor Edward Amoruso
                        </Sponsor>
                    </Line>
                    <Grid container justifyContent={"center"} columns={20} >
                        {v3devs.map((dev) => {
                                return(
                                    <Grid item align="center"  sm={"auto"} md={6} lg={"auto"}>
                                        <DevCard dev = {dev}/>
                                    </Grid>
                                )
                        })}
                    </Grid>
                </Developers>
                <Developers>
                    <Line>
                        <Subtitle>
                            SeeShells V2
                        </Subtitle>
                        <Sponsor>
                            Sponsor: Dr. Richard Leinecker
                        </Sponsor>
                    </Line>
                    <Grid container justifyContent={"center"} columns={20} >
                        {v2devs.map((dev) => {
                                return(
                                    <Grid item align="center"  sm={"auto"} md={6} lg={"auto"}>
                                        <DevCard dev = {dev}/>
                                    </Grid>
                                )
                        })}
                    </Grid>
                </Developers>
                <Developers>
                    <Line>
                        <Subtitle>
                            SeeShells V1
                        </Subtitle>
                        <Sponsor>
                            Sponsor: Dr. Richard Leinecker
                        </Sponsor>
                    </Line>
                    <Grid container justifyContent={"center"} columns={20} >
                        {v1devs.map((dev) => {
                                return(
                                    <Grid item align="center"  sm={"auto"} md={6} lg={"auto"}>
                                        <DevCard dev = {dev}/>
                                    </Grid>
                                )
                        })}
                    </Grid>
                </Developers>
            </MainContent>
        </Contain>
    )
}