import React from 'react';
import { withStyles } from '@material-ui/core/styles';
import { withRouter } from 'react-router-dom';

const styles = {
};

class DeveloperPage extends React.Component {
    render() {
        return(
            <p>placeholder</p>
        );
    }
}

export default withStyles(styles)(withRouter(DeveloperPage));