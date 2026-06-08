import {
    Component
}
from "react";

import type {
    ReactNode
}
from "react";

type Props = {
    children: ReactNode;
};

type State = {
    hasError: boolean;
};

export default class ErrorBoundary
    extends Component<Props, State>
{
    constructor(props: Props)
    {
        super(props);

        this.state = {
            hasError: false
        };
    }

    static getDerivedStateFromError()
    {
        return {
            hasError: true
        };
    }

    componentDidCatch(
        error: Error,
        errorInfo: React.ErrorInfo)
    {
        console.error(
            error,
            errorInfo
        );
    }

    render()
    {
        if (this.state.hasError)
        {
            return (
                <div className="page">

                    <h1>
                        Unexpected Error
                    </h1>

                    <p>
                        An unexpected application
                        error occurred.
                    </p>

                    <p>
                        Please refresh the page.
                    </p>

                </div>
            );
        }

        return this.props.children;
    }
}