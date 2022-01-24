import * as React from 'react';
import { Route } from 'react-router';
import Layout from './components/Layout';
import Home from './components/Home';

import Todo from './components/Todo';
import Categories from './components/Categories';
import Category from './components/Category';

import './custom.css'

export default () => (
    <Layout>
        <Route exact path='/' component={Home} />
        <Route path='/categories' component={Categories} />
        <Route path='/category' component={Category} />
        <Route path='/todo' component={Todo} />
    </Layout>
);
