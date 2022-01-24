import * as React from 'react';
import { connect } from 'react-redux';
import { RouteComponentProps } from 'react-router';
import * as CategoriesStore from '../store/Categories';


export default () => {
    return (
        <div className="d-flex flex-column flex-shrink-0 p-3 bg-light">
            <ul className="nav nav-pills flex-column mb-auto">
                <li className="nav-item">
                    <a href="#" className="nav-link" aria-current="page">
                        Category one
                    </a>
                </li>
                <li className="nav-item active">
                    <a href="#" className="nav-link" aria-current="page">
                        Category Two
                    </a>
                </li>
                <li className="nav-item">
                    <a href="#" className="nav-link" aria-current="page">
                        Category third
                    </a>
                </li>
            </ul>
        </div>
    );
}


