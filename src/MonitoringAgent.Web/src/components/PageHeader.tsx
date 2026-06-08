import "./PageHeader.css";

type Props = {
    title: string;
    children?: React.ReactNode;
};

export default function PageHeader({
    title,
    children
}: Props) {
    return (
        <div className="page-header">

            <h1 className="page-title">
                {title}
            </h1>

            {children}

        </div>
    );
}