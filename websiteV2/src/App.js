import MenuBar from './components/MenuBar.js';
import FrontPage from './containers/FrontPage.js';
import AboutPage from './containers/AboutPage.js';
import DownloadPage from './containers/DownloadPage.js';
import DocumentationPage from './containers/DocumentationPage.js';
import DeveloperPage from './containers/DeveloperPage.js';
import Footer from './components/Footer.js';
import { withStyles } from '@material-ui/core/styles';
import { HashRouter as Router, Route } from 'react-router-dom';
import Paper from '@material-ui/core/Paper';

const styles = {
  application: {
    height: '100%',
    width: '100%',
    display: 'flex',
    flexDirection: 'column',
    flex: '1',
  },
  topBar: {
    color: 'white',
    display: 'flex',
    margin: '0px',
  },
  bottomBar: {
    color: 'white',
    display: 'flex',
    margin: '0px',
    width: '100%',
    marginTop: 'auto',
  },
  content: {
    color: 'black',
    display: 'flex',
    width: '100%',
    height: '100%',
  },
};

function App(props) {
  return (
    <Paper className={props.classes.application}>
      <Router basename="/">
        <div className={props.classes.topBar}>
          <MenuBar/>
        </div>
        <Paper className={props.classes.content}>
          <Route exact path="/">
            <FrontPage/>
          </Route>

          <Route exact path="/about">
            <AboutPage subpage="about"/>    
          </Route>
          <Route exact path="/about/registry">
            <AboutPage subpage="registry"/>
          </Route>
          <Route exact path="/about/shellbags">
            <AboutPage subpage="shellbags"/>
          </Route>
          <Route exact path="/about/seeshells">
            <AboutPage subpage="seeshells"/>
          </Route>
          <Route exact path="/about/parser">
            <AboutPage subpage="parser"/>
          </Route>
          <Route exact path="/about/analysis">
            <AboutPage subpage="analysis"/>
          </Route>
          <Route exact path="/about/timeline">
            <AboutPage subpage="timeline"/>
          </Route>
          <Route exact path="/about/filters">
            <AboutPage subpage="filters"/>
          </Route>
          <Route exact path="/about/case-studies">
            <AboutPage subpage="case-studies"/>
          </Route>

          <Route exact path="/download">
            <DownloadPage/>
          </Route>

          <Route exact path="/documentation">
            <DocumentationPage/>
          </Route>
          <Route exact path="/documentation/online">
            <DocumentationPage subpage="online"/>
          </Route>
          <Route exact path="/documentation/offline">
            <DocumentationPage subpage="offline"/>
          </Route>
          <Route exact path="/documentation/advanced">
            <DocumentationPage subpage="advanced"/>
          </Route>
          <Route exact path="/documentation/toolbar">
            <DocumentationPage subpage="toolbar"/>
          </Route>
          <Route exact path="/documentation/data">
            <DocumentationPage subpage="data"/>
          </Route>
          <Route exact path="/documentation/timeline">
            <DocumentationPage subpage="timeline"/>
          </Route>
          <Route exact path="/documentation/events">
            <DocumentationPage subpage="events"/>
          </Route>
          <Route exact path="/documentation/filters">
            <DocumentationPage subpage="filters"/>
          </Route>
          <Route exact path="/documentation/export">
            <DocumentationPage subpage="export"/>
          </Route>
          <Route exact path="/documentation/import">
            <DocumentationPage subpage="import"/>
          </Route>
          <Route exact path="/documentation/licensing">
            <DocumentationPage subpage="licensing"/>
          </Route>

          <Route exact path="/developers">
            <DeveloperPage/>
          </Route>
        </Paper>
        <div className={props.classes.bottomBar}>
          <Footer/>
        </div>
      </Router>
    </Paper>
  );
}

export default withStyles(styles)(App);
