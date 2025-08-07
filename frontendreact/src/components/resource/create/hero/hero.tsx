'use client'

import { useRouter } from 'next/navigation';
import { useState } from 'react';
import styles from '@/styles/recipts-page/hero/hero.module.css';

export default function Hero() {
    const [inputNumber, setInputNumber] = useState('');
    return(
        <main className={styles.mainContainer}>
            <h1 className={styles.headerText}>Ресурс</h1>
            <div className={styles.buttonGroup}>
                <button className={styles.addButton}>Сохранить</button>
            </div>
            <div className={styles.filterBox}>
                <h4 className={styles.filterText}>Наименование</h4>
                <div className={styles.inputGroup}>
                    <input
                    type='text'
                    className={styles.inputNumber}
                    value={inputNumber}
                    onChange={(e) => setInputNumber(e.target.value)}
                    />
                </div>
            </div>
        </main>
    )
}