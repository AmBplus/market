const express = require('express');
const path = require('path');
const app = express();
const PORT = 3611;

// ── Serve static files ──────────────────────────────────────
app.use(express.static(path.join(__dirname)));
app.use('/node_modules', express.static(path.join(__dirname, 'node_modules')));

// ── Mock Data ───────────────────────────────────────────────
const users = [
  { id: 1, name: 'Alice Johnson',  role: 'Admin' },
  { id: 2, name: 'Bob Smith',      role: 'Editor' },
  { id: 3, name: 'Charlie Brown',  role: 'Viewer' },
  { id: 4, name: 'Diana Prince',   role: 'Admin' },
  { id: 5, name: 'Edward Norton',  role: 'Editor' },
  { id: 6, name: 'Fiona Apple',    role: 'Viewer' },
  { id: 7, name: 'George Lucas',   role: 'Admin' },
  { id: 8, name: 'Hannah Montana', role: 'Editor' },
  { id: 9, name: 'Ivan Drago',     role: 'Viewer' },
  { id: 10, name: 'Julia Roberts', role: 'Admin' },
  { id: 11, name: 'Kevin Hart',    role: 'Editor' },
  { id: 12, name: 'Laura Palmer',  role: 'Viewer' },
];

const cityNames = [
  'New York','Los Angeles','Chicago','Houston','Phoenix','Philadelphia',
  'San Antonio','San Diego','Dallas','San Jose','Austin','Jacksonville',
  'Fort Worth','Columbus','Charlotte','Indianapolis','San Francisco',
  'Seattle','Denver','Washington','Nashville','Oklahoma City','El Paso',
  'Boston','Portland','Las Vegas','Memphis','Louisville','Baltimore','Milwaukee',
];
const cities = cityNames.map((city, i) => ({ id: i + 1, city }));

const countries = [
  { id: 1, name: 'United States' },
  { id: 2, name: 'Canada' },
  { id: 3, name: 'Germany' },
  { id: 4, name: 'France' },
  { id: 5, name: 'Japan' },
];

const depCities = {
  1: [{ id: 101, name: 'New York' }, { id: 102, name: 'Los Angeles' }, { id: 103, name: 'Chicago' }],
  2: [{ id: 201, name: 'Toronto' }, { id: 202, name: 'Vancouver' }, { id: 203, name: 'Montreal' }],
  3: [{ id: 301, name: 'Berlin' }, { id: 302, name: 'Munich' }, { id: 303, name: 'Hamburg' }],
  4: [{ id: 401, name: 'Paris' }, { id: 402, name: 'Lyon' }, { id: 403, name: 'Marseille' }],
  5: [{ id: 501, name: 'Tokyo' }, { id: 502, name: 'Osaka' }, { id: 503, name: 'Kyoto' }],
};

const products = [
  { id: 1, label: 'Laptop Pro 15"',    category: 'Electronics', price: 1299 },
  { id: 2, label: 'Wireless Mouse',    category: 'Electronics', price: 29 },
  { id: 3, label: 'Mechanical Keyboard', category: 'Electronics', price: 89 },
  { id: 4, label: 'USB-C Hub',         category: 'Electronics', price: 49 },
  { id: 5, label: 'Standing Desk',     category: 'Furniture',   price: 399 },
  { id: 6, label: 'Ergonomic Chair',   category: 'Furniture',   price: 299 },
  { id: 7, label: 'Monitor 27"',       category: 'Electronics', price: 349 },
  { id: 8, label: 'Desk Lamp',         category: 'Furniture',   price: 45 },
  { id: 9, label: 'Notebook A5',       category: 'Stationery',  price: 8 },
  { id: 10, label: 'Pen Set',          category: 'Stationery',  price: 12 },
  { id: 11, label: 'Whiteboard',       category: 'Stationery',  price: 65 },
  { id: 12, label: 'Cable Organizer',  category: 'Electronics', price: 19 },
];

// ── Helpers ─────────────────────────────────────────────────
function filterByTerm(arr, key, term) {
  if (!term) return arr;
  const t = term.toLowerCase();
  return arr.filter(item => item[key].toLowerCase().includes(t));
}

function paginate(arr, page, limit) {
  const start = (page - 1) * limit;
  return arr.slice(start, start + limit);
}

// ── API Routes ───────────────────────────────────────────────

// GET /api/users?search=&id=
app.get('/api/users', (req, res) => {
  const { search = '', id } = req.query;
  let result = users;
  if (id) {
    result = users.filter(u => String(u.id) === String(id));
  } else {
    result = filterByTerm(users, 'name', search);
  }
  res.json({ data: result });
});

// GET /api/cities?q=&page=&limit=
app.get('/api/cities', (req, res) => {
  const term  = req.query.q || req.query.search || '';
  const page  = parseInt(req.query.page, 10) || 1;
  const limit = parseInt(req.query.limit || req.query.pageSize, 10) || 5;
  const filtered = filterByTerm(cities, 'city', term);
  const paged    = paginate(filtered, page, limit);
  res.json({ data: { items: paged, total: filtered.length } });
});

// GET /api/countries?search=
app.get('/api/countries', (req, res) => {
  const { search = '' } = req.query;
  res.json({ data: filterByTerm(countries, 'name', search) });
});

// GET /api/dep-cities?countryId=&search=
app.get('/api/dep-cities', (req, res) => {
  const { countryId = '', search = '' } = req.query;
  const list = depCities[countryId] || [];
  res.json({ data: filterByTerm(list, 'name', search) });
});

// GET /api/products?search=&page=&pageSize=&id=
app.get('/api/products', (req, res) => {
  const { search = '', id } = req.query;
  const page     = parseInt(req.query.page, 10) || 1;
  const pageSize = parseInt(req.query.pageSize, 10) || 5;
  let result = products;
  if (id) {
    result = products.filter(p => String(p.id) === String(id));
    return res.json({ data: result });
  }
  result = filterByTerm(products, 'label', search);
  const paged = paginate(result, page, pageSize);
  res.json({ data: { items: paged, total: result.length } });
});

// ── Start ────────────────────────────────────────────────────
app.listen(PORT, () => {
  console.log(`\n  Select2 Attribute Plugin demo running at:`);
  console.log(`  http://localhost:${PORT}/demo.html\n`);
});
