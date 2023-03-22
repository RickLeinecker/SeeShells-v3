import React from "react";
import {Routes, Route, BrowserRouter} from "react-router-dom";
import {useState, useEffect} from 'react';
import About from "./About";
import Devs from "./Devs";
import CaseStudies from "./CaseStudies"
import HowToUse from "./HowToUse";
import { StyledEngineProvider } from '@mui/material/styles';

export default function Nav()
{
    const [windowSize, setWindowSize] = useState({
        width: window.innerWidth,
        height: window.innerHeight,
      });
    
      useEffect(() => {
        window.onresize = () => {
          setWindowSize({
            width: window.innerWidth,
            height: window.innerHeight,
          });
        };
      }, []);

      console.log(windowSize.width)

    return(
      <StyledEngineProvider injectFirst>
        <BrowserRouter>
            <Routes>
                <Route path="/" element={<About size ={windowSize}/>} />
                <Route path="/Developers" element={<Devs size ={windowSize}/>} />
                <Route path="/HowToUse" element={<HowToUse size={windowSize} />} />
                <Route path="/CaseStudies" element={<CaseStudies size={windowSize} />} />
           </Routes>
        </BrowserRouter>
      </StyledEngineProvider>
    )
}