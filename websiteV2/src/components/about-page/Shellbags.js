import React from 'react';
import { withStyles } from '@material-ui/core/styles';
import { Typography, Paper } from '@material-ui/core';
import ReactPlayer from "react-player";

const styles = {
    content: {
        height: '100%',
        width: '100%',
        display: 'flex',
        justifyContent: 'flex-start',
        flexDirection: 'column',
        alignItems: 'center',
        overflow: 'auto',
        borderRadius: '0',
    },
    title: {
        fontSize: '50px',
        fontWeight: 'bold',
        marginTop: '1%',
        alignSelf: 'center',
        color: '#33A1FD',
        textAlign: 'center',
    },
    text: {
        textAlign: 'center',
        padding: '1%',
    },
    video: {
        maxHeight: '360px',
        maxWidth: '640px',
        minHeight: '150px',
        minWidth: '300px',
        height: '50%',
        width: '50%',
        margin: '10px',
        display: 'flex',
        flexDirection: 'column',
        alignItems: 'center',
        backgroundColor: '#424242',
    },
}

class Shellbags extends React.Component {

    render() {
        return (
            <div className={this.props.classes.content}>
                <Typography variant="title" className={this.props.classes.title}>Shellbags</Typography>
                <Typography variant="subtitle1" className={this.props.classes.text}>
                    Below is a video discussing shellbag data and what SeeShells does with these shells.
                </Typography>
                <Paper className={this.props.classes.video}>
                    <ReactPlayer height='100%' width='100%' url="https://www.youtube.com/watch?v=_C23eW836eI"/>
                </Paper>
            </div>
        )
    }
}

export default withStyles(styles)(Shellbags);